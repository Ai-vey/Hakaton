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
using System.Speech.Recognition;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using System.IO;
using NAudio.Wave.SampleProviders;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // WaveIn - поток для записи
        WaveIn waveIn;
        //Класс для записи в файл
        WaveFileWriter writer;
        //Имя файла для записи
        string outputFilename = "file.wav";

        public MainWindow()
        {
            InitializeComponent();

        }

        byte[] array = null;
        byte[] array_output = null;

        //Завершение записи
        void StopRecording()
        {
            MessageBox.Show("Stop");
            waveIn.StopRecording();
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                writer.WriteData(e.Buffer, 0, e.BytesRecorded);
                
            }
            
        }

        private void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
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
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
                //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                waveIn.WaveFormat = new WaveFormat(8000, 1);
                //Инициализируем объект WaveFileWriter
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                //Начало записи
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (waveIn != null)
            {
                StopRecording();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            array = File.ReadAllBytes("./file.wav");
            AppData.Context.symbols.Add(new symbols
            {
                Pronunciation = array,
                Word = Txt_Inp.Text
            });
            AppData.Context.SaveChanges();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var _curentWord = AppData.Context.symbols.ToList().First(p => p.Word == Txt_Out.Text);
            if (array_output != _curentWord.Pronunciation)
            {

                array_output = _curentWord.Pronunciation;
                File.WriteAllBytes(path: "Audio.wav", array_output);
                Play();
            }
            else
            {
                Play();
            }

        }

        public static void Play()
        {
            var file = new AudioFileReader("Audio.wav");
            var trimmed = new OffsetSampleProvider(file);

            var player = new WaveOutEvent();
            player.Init(trimmed);
            player.Play();
        }

    }


}

