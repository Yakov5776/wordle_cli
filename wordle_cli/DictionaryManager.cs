namespace wordle_cli
{
    internal class DictionaryManager
    {
        private string[] _wordList;
        public string[] wordList { get { return _wordList; } }

        public DictionaryManager()
        {
            _wordList = Utils.ReadResource<Program>("wordlist.txt").Split();
        }
    }
}
