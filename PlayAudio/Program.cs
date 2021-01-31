using NAudio.Wave;
using System;
using System.Threading;

namespace PlayAudio
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("PlayAudio v1.0");
                Console.WriteLine();
                Console.WriteLine("PlayAudio <SoundFile> <AudioDevice>");
                Console.WriteLine(" <SoundFile> can be wav or mp3");
                Console.WriteLine(" <AudioDevice> is either GUID of the device or parts of the device name");
                Console.WriteLine();
                Console.WriteLine("Available devices:");
                foreach (var dev in DirectSoundOut.Devices)
                {
                    Console.WriteLine($"* {dev.Description} --- GUID: {dev.Guid}");
                }
                return;
            }

            string audioFileName = args[0];
            string DeviceName = args[1];

            Guid deviceGuid = new Guid();
            foreach (var dev in DirectSoundOut.Devices)
            {
                if (dev.Description.Contains(DeviceName))
                {
                    deviceGuid = dev.Guid;
                    Console.WriteLine($"{dev.Guid} {dev.ModuleName} -  {dev.Description}");
                    break;
                }
            }

            // nach GUID suchen
            if (deviceGuid.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                foreach (var dev in DirectSoundOut.Devices)
                {
                    if (dev.Guid.ToString().Contains(DeviceName))
                    {
                        deviceGuid = dev.Guid;
                        Console.WriteLine($"{dev.Guid} {dev.ModuleName}");
                        Console.WriteLine($"{dev.Description}");
                        break;
                    }
                }
            }
            Console.WriteLine("Playing: " + audioFileName);
            
            using (var audioFile = new AudioFileReader(audioFileName))
            using (var outputDevice = new DirectSoundOut(deviceGuid))
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
