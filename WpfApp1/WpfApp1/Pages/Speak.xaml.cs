using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Speech.AudioFormat;
using System.Globalization;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Speak.xaml
    /// </summary>
    public partial class Speak : Page
    {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer("имя_файла.wav");
        public Speak()
        {
            InitializeComponent();

            
        }
        
        private void SpeakBtn_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer speak = new SpeechSynthesizer();
            var voiceList = speak.GetInstalledVoices();
            speak.SelectVoice(voiceList[0].VoiceInfo.Name);
            speak.Rate = 0;
            speak.SpeakAsync(SpeakTxt.Text);
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            SpeechRecognitionEngine _rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            _rec.LoadGrammar(new DictationGrammar());
            _rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            _rec.SetInputToDefaultAudioDevice();
            _rec.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SpeakTxt.Text = e.Result.Text;
        }
    }
}
