using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CSAudioRecorder;
using System.Timers;

namespace WebApplication1
{
    public partial class AudioTestingForm : System.Web.UI.Page
    {
        public CSAudioRecorder.AudioRecorder audioRecorder1 = new AudioRecorder();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioTestingForm()
        {

            this.audioRecorder1 = new CSAudioRecorder.AudioRecorder();
            /*
            this.audioRecorder1.RecordProgress += new CSAudioRecorder.OnRecordProgressEventHandler(this.a);
            this.audioRecorder1.RecordError += new CSAudioRecorder.OnRecordErrorEventHandler(this.audioRecorder1_RecordError);
            this.audioRecorder1.RecordDone += new System.EventHandler(this.audioRecorder1_RecordDone);
            this.audioRecorder1.RecordStart += new System.EventHandler(this.audioRecorder1_RecordStart);
            */
        }

        /// <summary>
        /// Start to record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRecord_Click(object sender, EventArgs e)
        {
            //audioRecorder1.Bitrate = (CSAudioRecorder.Bitrate)Enum.Parse(typeof(CSAudioRecorder.Bitrate), cboBitrate.Text);

            //[Optional: Default is 44100]
            //Set the sample rate to record from the device and the destination audio file:
           // audioRecorder1.Samplerate = (CSAudioRecorder.Samplerate)Enum.Parse(typeof(CSAudioRecorder.Samplerate), cboSamplerate.Text);
            //Or set by value
            //audioRecorder1.SamplerateVal = 48000;

            //Check if the control is already recording, if so, stop the recording operation before starting.
            if (audioRecorder1.RecordingState == CSAudioRecorder.RecordingState.Recording)
            {
                audioRecorder1.Stop();

                while (audioRecorder1.RecordingState != CSAudioRecorder.RecordingState.Ready)
                { 

                    Thread.Sleep(100);
                }
            }
           
            //Set the destination file name and path:
            //SetDestinationFileName();

            //Set the destination file:
            //audioRecorder1.FileName = lblDestinationFile.Text;

            //Set the audio device to record from:
            //audioRecorder1.AudioSource = cboAudioSource.Text;

            //Set the audio device index to record from:
           // audioRecorder1.DeviceIndex = cboAudioSource.SelectedIndex;

            //[Optional: Default is MP3]
            //The format of the destination file, this can be AAC, ACM(WAV), APE, MP2, MP3, OGG, WAV(PCM), and WMA.
           // audioRecorder1.Format = (CSAudioRecorder.Format)Enum.Parse(typeof(CSAudioRecorder.Format), cboDestinationFormat.Text);

            #region MoreOptionalProperties

            //[Optional: Default is 44100]
            //Set the sample rate to record from the device and the destination audio file:
            //audioRecorder1.Samplerate = (CSAudioRecorder.Samplerate)Enum.Parse(typeof(CSAudioRecorder.Samplerate), cboSamplerate.Text);
            //Or set by value
            //audioRecorder1.SamplerateVal = 48000;

            //[Optional: Default is 16]
            //Set the bit-depth to record from the device and the destination audio file:
            //audioRecorder1.Bits = (CSAudioRecorder.Bits)Enum.Parse(typeof(CSAudioRecorder.Bits), cboBits.Text);

            //[Optional: Default is 2]
            //Set the number of the channels to record from the device and the destination audio file:
           // audioRecorder1.Channels = (CSAudioRecorder.Channels)Enum.Parse(typeof(CSAudioRecorder.Channels), cboChannels.Text);

            //[Optional: Default is WasapiLoopbackCapture]
            //The mode of the recording process. This can be WasapiLoopbackCapture(default), WasapiCapture or LineIn:
            //audioRecorder1.Mode = (CSAudioRecorder.Mode)Enum.Parse(typeof(CSAudioRecorder.Mode), cboRecorderMode.Text);

            //[Optional: Default is 100]
            //The latency of the capture in milliseconds. The default value is 100, some sound card devices need an high value in order to avoid glitches:
            audioRecorder1.Latency = 100;

            //[Optional: Default is nothing]
            //Set the audioVisualization control to display the audio graph:
            //audioRecorder1.AudioVisualization = audioVisualization1;

            //[Optional: Default is nothing]
           // audioRecorder1.AudioMeter = audioMeter1;

            /****************************************
                Start to record when noise is detected:
            ****************************************/

            //[Optional: Default is false]
            //Start to record only if a noise is detect:
            //audioRecorder1.StartOnNoise = chkStartWhenNoise.Checked;

            //[Optional: Default is false]
            //Set the noise value (of the meter) to start the record 
           // audioRecorder1.StartOnNoiseVal = float.Parse(txtStartWhenNoiseVal.Text);

            /****************************************
                Stop to record when silence is detected:
            ****************************************/

            //[Optional: Default is false]
            //Stop the recording operation when silence is detected:
            //audioRecorder1.StopOnSilence = chkStopWhenSilence.Checked;

            //[Optional: Default is 0]
            //Set the minimum meter value of the silence, this can be a value between 1 to 100:
            //audioRecorder1.StopOnSilenceVal = float.Parse(txtStopWhenSilenceVal.Text);

            //[Optional: Default is 3]
            //Set the number of seconds to detect the silence:
           // audioRecorder1.StopOnSilenceSeconds = int.Parse(txtStopWhenSilenceSeconds.Text);

            #endregion

            //Start to record:
            audioRecorder1.Start();

            //Start the timer:
            //tmrMeterIn.Interval = 40;
            //tmrMeterIn.Enabled = true;

        }

        /// <summary>
        /// Get and set the audio devices according to the capture mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRecorderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the selected capture mode:
           // CSAudioRecorder.Mode mode = (CSAudioRecorder.Mode)Enum.Parse(typeof(CSAudioRecorder.Mode), cboRecorderMode.Text);

            //Get the audio devices:
           // cboAudioSource.DataSource = audioRecorder1.GetDevices(mode);

            //Try to set the default device:
           // int default_device_index = audioRecorder1.GetDeviceDefaultIndex(mode);

            //if (default_device_index != -1)
                //Set the default device index:
                //cboAudioSource.SelectedIndex = default_device_index;
           // else
                //LineIn will always return -1:
               // cboAudioSource.SelectedIndex = 0;

        }

        /// <summary>
        /// Set the destination file. This example use the default music folder of Windows.
        /// </summary>
        private void SetDestinationFileName()
        {
            //Get the default music directory of Windows:
           // string sDestinationFile = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\out." + cboDestinationFormat.Text.ToLower();

            //If the destination is ACM add the .WAV ext:
           // if (cboDestinationFormat.Text == "ACM")
                //sDestinationFile = sDestinationFile + ".wav";

            //Set the lblDestinationFile:
           // lblDestinationFile.Text = sDestinationFile;

            //Set the destination file of the control:
            //lblDestinationFile.Text = sDestinationFile;
        }

        /// <summary>
        /// Stop the recording operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStop_Click(object sender, EventArgs e)
        {
            //Check if the control is recording, if not do nothing:
            if (audioRecorder1.RecordingState == CSAudioRecorder.RecordingState.Ready)
                return;

            //Stop the meter timer:
            //tmrMeterIn.Enabled = false;

            //Stop to record:
            audioRecorder1.Stop();
        }

        /// <summary>
        /// Pause the recording operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPause_Click(object sender, EventArgs e)
        {
            audioRecorder1.Pause();
        }

        /// <summary>
        /// UnPause the recording operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdUnPause_Click(object sender, EventArgs e)
        {
            audioRecorder1.UnPause();
        }

        /// <summary>
        /// Timer tick to get the meter. You can also get the meter value in RecordProgress event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrMeterIn_Tick(object sender, EventArgs e)
        {
            //Get the meter:
           // lblMeterIn.Text = audioMeter1.Meter.ToString("0.00");
        }

        /// <summary>
        /// On closing, check if the control is recording, if so, stop the recording operation before exit the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void frmAudioRecorder_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region SafeExit

            //Safe exit
            if (AudioRecorder.RecordingState == CSAudioRecorder.RecordingState.Recording)
            {
                audioRecorder1.Stop();

                while (audioRecorder1.RecordingState != CSAudioRecorder.RecordingState.Ready)
                {
                    Thread.Sleep(100);
                }
            }

            #endregion
        }
        */
        /// <summary>
        /// When done event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioRecorder1_RecordDone(object sender, EventArgs e)
        {
            //Set the ID3 tags:

            //You can set can set and edit the ID3 tags of the following formats:
            //MP3, MPC, FLAC, MP4, ASF, AIFF, TrueAudio, WavPack, Ogg FLAC, Ogg Vorbis, Speex and Opus file formats.
        }

        /// <summary>
        /// On error event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioRecorder1_RecordError(object sender, CSAudioRecorder.MessageArgs e)
        {
            //MessageBox.Show(e.seconds.ToString());
            Console.WriteLine(e.Number);
            Console.WriteLine(e.String);
        }

        /// <summary>
        /// On start event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioRecorder1_RecordStart(object sender, EventArgs e)
        {
            Console.WriteLine("Start recording...");
        }

        /// <summary>
        /// On progress event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioRecorder1_RecordProgress(object sender, CSAudioRecorder.ProgressArgs e)
        {
            //lblSizeIn.Text = audioRecorder1.TotalSizeIn; // Or audioRecorder1.SizeSuffix(e.Bytes);
            //lblTimeIn.Text = audioRecorder1.TotalTimeIn; // Or TimeSpan.FromSeconds(e.Seconds).ToString();
        }
    }
}
