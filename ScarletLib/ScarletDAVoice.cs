using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;


namespace ScarletLib
{
    public class ScarletDAVoice
    {
        private static readonly ScarletDAVoice instance = new ScarletDAVoice();
        static SpeechSynthesizer _synthesizer;
        private ScarletDAVoice()
        {}
        public void StartMorning()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.Speak("Good Morning Sir! How did you sleep?");
        }
        public static ScarletDAVoice Voice
        {
            get {
                return instance;      
        }
            }
        }
    
}
