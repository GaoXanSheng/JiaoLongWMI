using JiaoLongWMI.Models;

namespace JiaoLongWMI.tools;

using System.Drawing;
using System.Threading.Tasks;

class RGBBreathingLightEffect
{
    // Create threads to run the effects synchronously
    // private Thread breathingEffectThread;
    private Thread gradientEffectThread ;
    // Usage example:
    public RGBBreathingLightEffect()
    {
        // breathingEffectThread = new Thread(ApplyBreathingLightEffect);
        gradientEffectThread = new Thread(ApplyLoopingGradientColor);
    }

    public void Start()
    {
        // Start the threads
        // breathingEffectThread.Start();
        gradientEffectThread.Start();
    }
    // Stop the threads
    public void Stop()
    {
        running = false;
    }
    private bool running = true;

    // Simulating LightBrightnessSet function
    private void LightBrightnessSet(byte brightness)
    {
        // Add logic to set the light brightness here
        Keyboard.LightBrightness.Set(brightness);
        Thread.Sleep(50); // Simulate the delay
    }

    // Simulating RGB_Set function
    private void RGB_Set(byte r, byte g, byte b)
    {
        // Add logic to set the RGB color here
        Keyboard.Color.Set(r, g, b);
        Thread.Sleep(50); // Simulate the delay
    }

    // Apply the Breathing Light Effect
    private void ApplyBreathingLightEffect()
    {
        const byte minBrightness = 0; // Minimum brightness level
        const byte maxBrightness = 3; // Maximum brightness level
        const int delay = 500; // Delay in milliseconds between each brightness step

        while (running)
        {
            // Increase brightness
            for (byte brightness = minBrightness; brightness < maxBrightness; brightness++)
            {
                 LightBrightnessSet(brightness);
                 Thread.Sleep(delay); // Wait for the specified delay
            }

            // Decrease brightness
            for (byte brightness = maxBrightness; brightness > minBrightness; brightness--)
            {
                 LightBrightnessSet(brightness);
                 Thread.Sleep(delay); // Wait for the specified delay
            }
        }
    }

    // Get Gradient Color
    private Color GetGradientColor(Color startColor, Color endColor, float percent)
    {
        byte r = (byte)(startColor.R + (endColor.R - startColor.R) * percent);
        byte g = (byte)(startColor.G + (endColor.G - startColor.G) * percent);
        byte b = (byte)(startColor.B + (endColor.B - startColor.B) * percent);
        return Color.FromArgb(r, g, b);
    }
    private Color[] colors = new[] {
        Color.Red, // Red
        Color.Green, // Green
        Color.Blue  // Blue
    };
    // Apply Looping Gradient Color
    private void ApplyLoopingGradientColor()
    {
        const int steps = 100;
        const int delay = 50; // Milliseconds

        while (running)
        {
            for (int i = 0; i < colors.Length && running; i++)
            {
                var startColor = colors[i];
                var endColor = colors[(i + 1) % colors.Length];

                for (int j = 0; j <= steps && running; j++)
                {
                    float percent = (float)j / steps;
                    var color = GetGradientColor(startColor, endColor, percent);
                    RGB_Set(color.R, color.G, color.B);
                    Thread.Sleep(delay); // Wait for the specified delay
                }
            }
        }
    }
}

