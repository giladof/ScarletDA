using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;


namespace ScarletLib.BaseClasses
{
    public sealed class ScarletDAVoice
    {
        private static volatile ScarletDAVoice instance = new ScarletDAVoice();
        static SpeechSynthesizer _synthesizer;
        private static object syncRoot = new Object();
        private ScarletDAVoice()
        {}
        public void SpeakPhrase(string Phrase)
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            _synthesizer.Speak(Phrase);
        }
        public static ScarletDAVoice Voice
        {
            get {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScarletDAVoice();
                    }
                }   
                return instance;      
        }
            }
        }
    
}
