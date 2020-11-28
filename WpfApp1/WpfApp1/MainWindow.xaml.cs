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
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using System.Globalization;




namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        
        WaveFileWriter writer;

        string outputFilename = "имя_файла.wav";
        public MainWindow()
        {
            InitializeComponent();
            
        }

       

        public bool InvokeRequired { get; private set; }

        [Obsolete]
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                //Записываем данные из буфера в файл
                writer.WriteData(e.Buffer, 0, e.BytesRecorded);
            }
        }

        private void BeginInvoke(EventHandler<WaveInEventArgs> eventHandler, object sender, WaveInEventArgs e)
        {
            throw new NotImplementedException();
        }

        //Завершаем запись
        void StopRecording()
        {
            MessageBox.Show("StopRecording");
            waveIn.StopRecording();
        }
        //Окончание записи
        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }

        private void BeginInvoke(EventHandler eventHandler, object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                MessageBox.Show("Start Recording");
                waveIn = new WaveIn();
                //Дефолтное устройство для записи (если оно имеется)
                //встроенный микрофон ноутбука имеет номер 0
                waveIn.DeviceNumber = 0;
                //Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                waveIn.DataAvailable += waveIn_DataAvailable;
                //Прикрепляем обработчик завершения записи
                waveIn.RecordingStopped += waveIn_RecordingStopped;
                //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                waveIn.WaveFormat = new WaveFormat(8000, 1);
                //Инициализируем объект WaveFileWriter
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                //Начало записи
                waveIn.StartRecording();
                
                
                
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            if (waveIn != null)
            {
                StopRecording();
            }
            
        }

        private void SpeakBtn_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer speak = new SpeechSynthesizer();
            speak.SelectVoiceByHints(VoiceGender.Male,VoiceAge.Senior);
            speak.SpeakAsync(SpeakTxt.Text);
        }

        
    }
    
    
}
