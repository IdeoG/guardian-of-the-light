using System.Collections;

namespace _Guardian_of_the_Light.Scripts.UI.Vignette
{
    public interface IVignette
    {
        IEnumerator Collapse();
        IEnumerator Expand();
    }
}