using System.Drawing;

namespace Thingy.GraphicsPlusGui
{
    /// <summary>
    /// Interface to a class that provides a back buffer Image for a Graphics object, a
    /// graphics object for drawing operations and a Render method for rendering it
    /// back to the original Graphics object.
    /// </summary>
    public interface IGraphicsPlus
    {
        /// <summary>
        /// Creates a scaled up Image based on the passed graphics object
        /// </summary>
        /// <param name="graphics">The base graphics object</param>
        /// <param name="size">The size of the image</param>
        /// <returns>The scaled up graphics object</returns>
        Image CreateImage(Graphics graphics, SizeF size);

        /// <summary>
        /// Creates a graphics object with the PageScale set so that
        /// we can draw to the scaled bitmap as if it is the same
        /// dimensions as the original
        /// </summary>
        /// <param name="image">The scale up image</param>
        /// <returns>The graphics object</returns>
        Graphics CreateGraphics(Image image);

        /// <summary>
        /// Renders the source graphics object onto the target one applying any scaling
        /// that was applied when creating the source object
        /// </summary>
        /// <param name="source">The source Graphics object</param>
        /// <param name="target">The traget Graphics object</param>
        /// <param name="clipRect">The target clip rectangle</param>
        void Render(Image source, Graphics target, RectangleF clipRect);
    }
}