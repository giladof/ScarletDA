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
            Choices options = new Choices(new string[] { "Hey Scarlet","Thank you", "How do you feel?", "How are you?", "What's up?", "Tell the time", "Tell the date","What is the meaning of life?", "What is the answer to life the universe and everything?", "Bye bye Scarlet", "Goodbye Scarlet", "Goodbye" });
            g.Append(options);            
            _recognizer.LoadGrammar(new Grammar(g));
            GrammarBuilder gOpen = new GrammarBuilder("Open");
            Choices openProg = new Choices(new string[] {"Notepad", "google", "facebook" });
            gOpen.Append(openProg);
            _recognizer.LoadGrammar(new Grammar(gOpen));
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

        private void _recognizerSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string answer = e.Result.Text;
            if (answer == "Hey Scarlet")
            {
                isInit = true;
                return;
            }
            else if (answer == "Bye bye Scarlet"||answer== "Goodbye Scarlet"||answer=="Goodbye")
            {
                ScarletDAVoice.Voice.SpeakPhrase("Have a nice day, Goodbye!");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            if (isInit)
            {
                //Greeting
                if (answer == "How do you feel?" || answer == "How are you?" || answer == "What's up?")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("I feel great, thank you! Have a nice day");

                }
                else if (answer.StartsWith("Open"))
                {
                    switch (answer.Split(' ')[1])
                    {
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
                                    ScarletLogger.LogMessage("Unable to start broswer: "+ex2.InnerException.Message, AppDomain.CurrentDomain.BaseDirectory + "HudLog.txt");

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
                else if (answer == "Thank you")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("You're welcome");

                }
                else if (answer == "What is the meaning of life?" || answer == "What is the answer to life the universe and everything?")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("According to The Hitchhiker's Guide to the Galaxy, the answer is 42");
                }
                
              
                else if (answer == "Tell the time")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("It is now " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                }
                else if (answer == "Tell the date")
                {
                    ScarletDAVoice.Voice.SpeakPhrase("Today is  " + DateTime.Now.ToString("dddd") + " , The " + DateTime.Today.Day + " of " + DateTime.Now.ToString("MMMM"));
                }
                isInit = false;
            }
        }

        
    }
}
