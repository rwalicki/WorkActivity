namespace WorkActivity.WPF.Services.Renderer
{
    public interface IDocumentBuilder
    {
        IDocumentBuilder WithElement(string element);
        string Build();
    }
}