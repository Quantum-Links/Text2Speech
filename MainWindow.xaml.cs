using Microsoft.Win32;
using SpeechLib;
using System.Windows;
using System.Windows.Controls;

namespace TTS_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SpVoice sp = new SpVoice();
        string pitch= "<pitch absmiddle=\"0\"/>";
        public MainWindow()
        {
            InitializeComponent();
            ISpeechObjectTokens voices = sp.GetVoices();
            foreach (ISpeechObjectToken item in voices)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.GetDescription();
                Speaker.Items.Add(comboBoxItem);
            }
            ComboBoxItem cbi = (Speaker.Items[0]) as ComboBoxItem;
            cbi.IsSelected = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sp.Speak(string.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            sp.Speak(pitch+Text2Talk.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void YinLiang_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sp.Volume =(int) e.NewValue*10;
        }

        private void YuSu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sp.Rate = (int)e.NewValue;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Title = "保存音频文件";
            file.Filter = "音频文件|*.wav";
            file.FilterIndex = 1;
            if (file.ShowDialog() == true)
            {
                SpFileStream spFileStream = new SpFileStream();
                spFileStream.Format.Type = SpeechAudioFormatType.SAFT16kHz16BitMono;
                spFileStream.Open(file.FileName, SpeechStreamFileMode.SSFMCreateForWrite, false);
                sp.AudioOutputStream = spFileStream; 
                sp.Speak(pitch+Text2Talk.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                sp.WaitUntilDone(-1);
                sp.AudioOutputStream = null;
                spFileStream.Close();
            }
        }
        private void YuDiao_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pitch = $"<pitch absmiddle=\"{(int)e.NewValue}\"/>";
        }
        private void Speaker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sp.Voice = sp.GetVoices().Item(Speaker.SelectedIndex);
        }
    }
}
