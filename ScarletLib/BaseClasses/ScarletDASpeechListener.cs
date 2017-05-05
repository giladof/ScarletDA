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
        private static volatile ScarletDASpeechListener instance = new ScarletDASpeechListener();
        static SpeechRecognitionEngine _recognizer;
        private static object syncRoot = new Object();
        static ScarletDADictionaryBase Programs;
        static ManualResetEvent _completed = null;
        static bool rejected = false;
        static string Suggestion;
        private bool _listen;

        private ScarletDASpeechListener()
        {
            _listen = true;
            Programs = new ScarletDADictionaryBase();
            Programs.AddProgramToDictonary(new ScarletDAProgram("Notepad", "notepad.exe", null));
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("Open Notepad")));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizerSpeechRecognized);
            _recognizer.SpeechRecognitionRejected += _recognizer_SpeechRecognitionRejected;
        }

        private void _recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (rejected)

            {
                return;
            }
            if (e.Result.Alternates.Count == 0)
            {
                ScarletDAVoice.Voice.SpeakPhrase("I'm sorry, I didn't understand the answer.");
                return;
            }
            rejected = true;
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
           
            if (answer == "Open Notepad")
            {
                
               bool res = Programs.RunProgram("Notepad").Result;
                ScarletDAVoice.Voice.SpeakPhrase("Program Opened");
            }
        }
    }
}
