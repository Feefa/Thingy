using System;
using System.Drawing;

namespace Thingy.GraphicsPlusGui
{
    /// <summary>
    /// Implementation of IGraphicsPlus which uses a scaled up bitmap as the back buffer Image.
    /// The Graphics object is set up to allow drawing operations to use the same units as the
    /// original Graphics objects even though its canvas is larger. The Render method scales the
    /// image back down, providing an anti-aliasing effect
    /// </summary>
    public class GraphicsPlus : IGraphicsPlus
    {
        /// <summary>
        /// Local variable for holding the configured scale parameter
        /// </summary>
        private readonly float scale;

        /// <summary>
        /// Creates and instance of the GraphicsPlus class.
        /// It's probably advisable to limit the value of scale to powers of two to
        /// maximise the efficiency of the Render function
        /// </summary>
        /// <param name="scale">The factor to scale the back buffer image by.</param>
        public GraphicsPlus(float scale)
        {
            this.scale = scale;
        }
        /// <summary>
        /// Creates a back buffer bitmap scaled according to the scale factor above using
        /// the resolution of the Graphics object representing the target for the final
        /// Render operation
        /// </summary>
        /// <param name="graphics">The target Graphics object</param>
        /// <param name="size">The size of the area to render (unscaled)</param>
        /// <returns>The back buffer Bitmap Image</returns>
        public Image CreateImage(Graphics graphics, SizeF size)
        {
            return new Bitmap(Convert.ToInt32(size.Width * scale), Convert.ToInt32(size.Height * scale), graphics);
        }

        /// <summary>
        /// Creates a Graphics object for the back buffer bitmap and configures it to accept
        /// positions and sizes in the same units as the target Graphics object regardless of
        /// the value of the scale factor
        /// </summary>
        /// <param name="image">The back buffer image</param>
        /// <returns>A Graphics object for operations on the back buffer image</returns>
        public Graphics CreateGraphics(Image image)
        {
            Graphics scaledUpGraphics = Graphics.FromImage(image);
            scaledUpGraphics.PageScale = scale;
            scaledUpGraphics.ScaleTransform(scale, scale);

            return scaledUpGraphics;
        }

        /// <summary>
        /// Draws the back buffer image using the operations provided by the target
        /// Graphics object
        /// </summary>
        /// <param name="source">The back buffer Image</param>
        /// <param name="target">The target Grapgics object</param>
        /// <param name="clipRect">The clipping rectangle</param>
        public void Render(Image source, Graphics target, RectangleF clipRect)
        {
            target.DrawImage(source, clipRect);
        }
    }
}
