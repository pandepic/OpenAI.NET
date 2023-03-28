namespace OpenAINET.Testbed
{
    public abstract class Test
    {
        public string APIKey;
        public OpenAIModelType ModelType;

        protected Test(string apiKey, OpenAIModelType modelType)
        {
            APIKey = apiKey;
            ModelType = modelType;
        }

        public virtual async Task AddUserInput(string input) { }
    }
}
