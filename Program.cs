using System;
using System.Collections.Generic;
using System.Text;
//Reference available at: https://github.com/cypherkey/RaspberryPi.Net
using RaspberryPiDotNet;
using System.Threading;

// Original author: Mikey Sklar - https://gist.github.com/3151375
// Adafruit article: https://raw.github.com/gist/3249416/7689f68f3ddbb74aceecda23e395c729668bd520/adafruit-cosm-temp.py
// Ported from python and modified by: Gilberto Garcia <ferraripr@gmail.com>; twitter: @ferraripr
namespace RPi_Temperature
{
    class Program
    {
        static void Main(string[] args)
        {
			//# set up the Serial Peripheral Interface(SPI) interface pins
            //# SPI port on the ADC to the Cobbler
			//clock pin
            GPIOMem SPICLK = new GPIOMem(GPIO.GPIOPins.GPIO18, GPIO.DirectionEnum.OUT);
			//SPI Master Output, Slave Input (MOSI)
			GPIOMem SPIMISO = new GPIOMem(GPIO.GPIOPins.GPIO23, GPIO.DirectionEnum.IN);
			//SPI Master Input, Slave Output (MISO)
            GPIOMem SPIMOSI = new GPIOMem(GPIO.GPIOPins.GPIO24, GPIO.DirectionEnum.OUT);
			//SPI Chip Select
            GPIOMem SPICS = new GPIOMem(GPIO.GPIOPins.GPIO25, GPIO.DirectionEnum.OUT);

            // temperature sensor connected to channel 0 of mcp3008
            int adcnum = 0;
            double read_adc0 = 0.0;

            while (true)
            {
				DateTime now = DateTime.Now;
                MCP3008 MCP3008 = new MCP3008(adcnum, SPICLK, SPIMOSI, SPIMISO, SPICS);
                // read the analog pin (temperature sensor LM35)
                read_adc0 = MCP3008.AnalogToDigital;
                double millivolts = Convert.ToDouble(read_adc0) * (3300.0 / 1024);

                double volts = (Convert.ToDouble(read_adc0) / 1024.0f) * 3.3f;
                double temp_C = ((millivolts - 100.0) / 10.0) - 40.0;
                double temp_F = (temp_C * 9.0 / 5.0) + 32;

#if DEBUG
                Console.WriteLine("MCP3008_Channel: " + adcnum);
                Console.WriteLine("read_adc0: " + read_adc0);
                Console.WriteLine("millivolts: " + (float)millivolts);
                Console.WriteLine("tempC: " + (float)temp_C);
                Console.WriteLine("tempF: " + (float)temp_F);
                Console.WriteLine("volts: " + (float)volts);
				//The following line makes the trick on Raspberry Pi for displaying DateTime.Now
				//equivalent.
				Console.WriteLine("Date time stamp: {0}/{1}/{2} {3}:{4}:{5}",now.Month,now.Day,now.Year,
				                  now.Hour,now.Minute,now.Second);
				System.Console.WriteLine("\n");
#endif
                Thread.Sleep(3000);
            }
        }
    }
}
