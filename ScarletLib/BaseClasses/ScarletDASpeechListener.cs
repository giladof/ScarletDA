using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScarletLib.BaseClasses
{
    public sealed class ScarletDASpeechListener
    {
        #region Init
        private static volatile ScarletDASpeechListener instance = new ScarletDASpeechListener();
        static SpeechRecognitionEngine _recognizer;
        private static object syncRoot = new Object();
        static ScarletDADictionaryBase Programs;
        static ManualResetEvent _completed = null;
        public  event EventHandler GoToSleep;
        static bool rejected = false;
        private bool _listen;
        private bool _init;
        #endregion

        public bool isInit
        {
            get { return _init; }
            
            private set { _init = value; }
        }

        private ScarletDASpeechListener()
        {
            _listen = true;
            Programs = new ScarletDADictionaryBase();
            Programs.AddProgramToDictonary(new ScarletDAProgram("Notepad", "notepad.exe", null));
            var progBrowser = new ScarletDAProgram("Browser", "\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\"", null);
            Programs.AddProgramToDictonary(progBrowser);
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.SetInputToDefaultAudioDevice();
            GrammarBuilder g = new GrammarBuilder();
            string[] generalChoises = new string[] {"I'm fine","Fine","I'm Good","I'm good, thanks", "Yes", "No", "Hey Scarlet", "Thank you", "Bye bye Scarlet", "Goodbye Scarlet", "Goodbye" };
            string[] questions =new string[] {"Go to sleep","sleep","Hi","Hey","Hello", "Who are you?", "Tell me about yourself", "How do you feel?", "How are you?", "What's up?", "Tell the time", "Tell the date", "What is the meaning of life?", "What is the answer to life the universe and everything?"};
            Choices options = new Choices(generalChoises);
            options.Add(questions);
            g.Append(options);
                        
            _recognizer.LoadGrammar(new Grammar(g));
            GrammarBuilder gOpen = new GrammarBuilder("Open");
            string[] openProgString = new string[] { "vmware", "Notepad", "google", "facebook", "incognito", "translate", "Visual Studio Code", "Visual Studio" };
            Choices openProg = new Choices(openProgString);
            gOpen.Append(openProg);
            Choices scarletAdd = new Choices();
            foreach (var v in openProgString)
            {
                scarletAdd.Add("Scarlet Open " + v);
            }
            foreach(var v in questions)
                scarletAdd.Add("Scarlet  " + v);

            _recognizer.LoadGrammar(new Grammar(gOpen));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(scarletAdd)));
            var scarletGrammar = new GrammarBuilder("Scarlet");
            
            _recognizer.LoadGrammar(new Grammar(scarletGrammar));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizerSpeechRecognized);
            _recognizer.SpeechRecognitionRejected += _recognizer_SpeechRecognitionRejected;
         
        }

        private void _recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (rejected)
            {
                return;
            }
            else
            {
                ScarletDAVoice.Voice.SpeakPhrase("I'm sorry, I didn't understand the answer.");
                rejected = true;
                return;
            }
            
        }

        public void StopListening()
        {
            _listen = false;
        }
        public void StartListening()
        {
            _listen = true;
        }

        public static ScarletDASpeechListener Listener
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScarletDASpeechListener();
                    }
                }
                
                return instance;
            }
        }
        public void Listen()
        {
            if (_listen)
            {
                rejected = false;
                _completed = new ManualResetEvent(false);
                _recognizer.Recognize(); // recognize speech asynchronous
            }
        }
        private static bool inAbout = false;
        private void _recognizerSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            string answer = e.Result.Text;
            if (answer.StartsWith("Scarlet"))
            {
                isInit = true;
            }
            if (inAbout)
            {
                inAbout = false;
                if (answer == "Yes")
                {
                    Programs.GetProgram("Browser").AddArgument(Properties.Resources.Developer);
                    bool res = Programs.RunProgram("Browser").Result;
                    ScarletDAVoice.Voice.SpeakPhrase("Profile opened");
                    Programs.GetProgram("Browser").RemoveArguments();
                }
                else if (answer == "No")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("Oh, Ok");
                }
            }
            if (answer == "Thank you")
            {
                ScarletDAVoice.Voice.SpeakPhrase("You're welcome");

            }
            else if (answer == "Hi" || answer == "Hey" || answer == "Hello")
            {
                ScarletDAVoice.Voice.SpeakPhrase("How are you?");
                _recognizer.SpeechRecognized -= _recognizerSpeechRecognized;
                _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizerAnswerGreeting);
                ScarletDASpeechListener.Listener.Listen();
            }
            else if (answer=="Who are you?")
            {
                ScarletDAVoice.Voice.SpeakPhrase("I am Scarlet, your digital assistant");
                ScarletDAVoice.Voice.SpeakPhrase("To ask me a question or give me commands, Say: Hey Scarlet!");
                ScarletDAVoice.Voice.SpeakPhrase("You can also say: scarlet, following a question or a command");
            }
            else if (answer == "Hey Scarlet")
            {
                isInit = true;
                return;
            }
            else if (answer == "Bye bye Scarlet" || answer == "Goodbye Scarlet" || answer == "Goodbye")
            {
                ScarletDAVoice.Voice.SpeakPhrase("Have a nice day, Goodbye!");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            if (isInit)
            {
                //Greeting
                if (answer.Contains("How do you feel?") || answer.Contains("How are you?") || answer.Contains("What's up?"))
                {
                    ScarletDAVoice.Voice.SpeakPhrase("I feel great, thank you! Have a nice day");

                }
                else if (answer.Contains("Open Visual Studio Code"))
                {
                    var result = new ScarletDAProgram("Visual Studio Code", "\"" + Properties.Resources.Visual_Studio_Code + "\"", null).RunmeAsync();
                    ScarletDAVoice.Voice.SpeakPhrase("Opening Visual Studio Code");
                }
                else if (answer.Contains("Open Visual Studio"))
                {
                    var result = new ScarletDAProgram("Visual Studio 2015", "\"" + Properties.Resources.Visual_Studio + "\"", null).RunmeAsync();
                    ScarletDAVoice.Voice.SpeakPhrase("Opening  Studio 2015");
                }
                else if (answer.Contains("Open vmware"))
                {
                    var result = new ScarletDAProgram("Vmware", "\"" + Properties.Resources.Vmware + "\"", null).RunmeAsync();
                    ScarletDAVoice.Voice.SpeakPhrase("Opening Vmware");
                }
                else if (answer.Contains("Go to sleep")||answer.Contains("Sleep"))
                {
                    ScarletDAVoice.Voice.SpeakPhrase("Ok, wake me up when you need me");
                    OnGotosleep(EventArgs.Empty);
                }
                else if (answer.Contains("Open"))
                {
                    switch (answer.Split(' ')[answer.Split(' ').Length - 1])
                    {
                        case "translate":
                            {
                                bool res = false;
                                try
                                {
                                    Programs.GetProgram("Browser").AddArgument(Properties.Resources.Google_Translate);
                                    res = Programs.RunProgram("Browser").Result;
                                    ScarletDAVoice.Voice.SpeakPhrase("Google translate opened");
                                    Programs.GetProgram("Browser").RemoveArguments();
                                }
                                catch (Exception ex1)
                                {
                                    ScarletLogger.LogMessage("Unable to start broswer: " + ex1.InnerException.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
                                }
                                break;
                            }
                        case "google":
                            {
                                bool res = false;
                                try
                                {
                                    Programs.GetProgram("Browser").AddArgument("google.com");
                                    res = Programs.RunProgram("Browser").Result;
                                    ScarletDAVoice.Voice.SpeakPhrase("Google opened");
                                    Programs.GetProgram("Browser").RemoveArguments();
                                }
                                catch (Exception ex1)
                                {
                                    ScarletLogger.LogMessage("Unable to start broswer: " + ex1.InnerException.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");
                                }
                                break;
                            }
                        case "incognito":
                            {
                                bool res = false;
                                try
                                {
                                    Programs.GetProgram("Browser").AddArgument("--incognito");
                                    res = Programs.RunProgram("Browser").Result;
                                    ScarletDAVoice.Voice.SpeakPhrase("Chrome opened in private mode");
                                    Programs.GetProgram("Browser").RemoveArguments();
                                }
                                catch (Exception ex2)
                                {
                                    ScarletLogger.LogMessage("Unable to start broswer: " + ex2.InnerException.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");

                                }
                                break;

                            }
                        case "facebook":
                            {
                                bool res = false;
                                try
                                {
                                    Programs.GetProgram("Browser").AddArgument("facebook.com");
                                    res = Programs.RunProgram("Browser").Result;
                                    ScarletDAVoice.Voice.SpeakPhrase("Facebook opened");
                                    Programs.GetProgram("Browser").RemoveArguments();
                                }
                                catch (Exception ex2)
                                {
                                    ScarletLogger.LogMessage("Unable to start broswer: " + ex2.InnerException.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");

                                }
                                break;

                            }
                        case "Notepad":
                            {
                                bool res = Programs.RunProgram("Notepad").Result;
                                ScarletDAVoice.Voice.SpeakPhrase("Program Opened");
                                break;
                            }
                    }
                }
                else if (answer.Contains("Tell me about yourself"))
                {
                    ScarletDAVoice.Voice.SpeakPhrase("My name is Scarlet, I am a digital assistant");
                    ScarletDAVoice.Voice.SpeakPhrase("I was developed by Guilad Ofir, in 2017");
                    ScarletDAVoice.Voice.SpeakPhrase("Would you like to view his Linked-in page?");
                    inAbout = true;
                    ScarletDASpeechListener.Listener.Listen();

                }

                else if (answer.Contains("What is the meaning of life?") || answer.Contains("What is the answer to life the universe and everything?"))
                {
                    ScarletDAVoice.Voice.SpeakPhrase("According to The Hitchhiker's Guide to the Galaxy, the answer is 42");
                }


                else if (answer.Contains("Tell the time"))
                {
                    string ampm = (DateTime.Now.Hour > 12 ? "PM" : "AM");
                    string hour = DateTime.Now.Hour.ToString();
                    if (ampm == "pm")
                    {
                        hour = (int.Parse(hour) - 12).ToString();
                    }
                    if (hour == "0")
                    {
                        hour = "12";
                    }
                    ScarletDAVoice.Voice.SpeakPhrase("It is now " + hour + " and " + DateTime.Now.Minute + " minutes " + ampm);
                }
                else if (answer.Contains("Tell the date"))
                {

                    ScarletDAVoice.Voice.SpeakPhrase("Today is  " + DateTime.Now.ToString("dddd") + " , " + DateTime.Now.ToString("MMMM") + " " + DateTime.Today.Day + GetOrdinalSuffix(DateTime.Today.Day) + " " + DateTime.Now.ToString("yyyy"));

                }
                isInit = false;
            }
        }

        private void _recognizerAnswerGreeting(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "I'm fine" || e.Result.Text == "Fine" || e.Result.Text == "I'm Good" || e.Result.Text == "I'm good, thanks")
            {
                ScarletDAVoice.Voice.SpeakPhrase("I'm glad to hear that");
            }
            _recognizer.SpeechRecognized -= _recognizerAnswerGreeting;
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizerSpeechRecognized);
        }

        /// <summary>
        /// Helper method for time
        /// Adding suffix to day
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string GetOrdinalSuffix(int num)
        {
            if (num.ToString().EndsWith("11")) return "th";
            if (num.ToString().EndsWith("12")) return "th";
            if (num.ToString().EndsWith("13")) return "th";
            if (num.ToString().EndsWith("1")) return "st";
            if (num.ToString().EndsWith("2")) return "nd";
            if (num.ToString().EndsWith("3")) return "rd";
            return "th";
        }
        private void OnGotosleep(EventArgs e)
        {
            EventHandler handler = GoToSleep;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
