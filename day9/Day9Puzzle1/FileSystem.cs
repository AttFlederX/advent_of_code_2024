class FileSystem
{
    private List<int?> _diskData = [];
    private List<int> _freeBlocks = [];

    public long Checksum => _diskData.Select((id, i) => (long)i * (id ?? 0)).Sum();

    public FileSystem(string diskMap)
    {
        var fileId = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            var blocks = diskMap[i] & 0xF;
            if (i % 2 == 0)
            {
                _diskData.AddRange(Enumerable.Repeat<int?>(fileId, blocks));
                fileId++;
            }
            else
            {
                _freeBlocks.AddRange(Enumerable.Range(_diskData.Count, blocks));
                _diskData.AddRange(Enumerable.Repeat<int?>(null, blocks));
            }
        }
    }

    public void Compact()
    {
        var freeBlockIdx = 0;
        for (int i = _diskData.Count - 1; i >= 0; i--)
        {
            var freeBlock = _freeBlocks[freeBlockIdx];
            if (freeBlock > i) break;

            if (_diskData[i] != null)
            {
                _diskData[freeBlock] = _diskData[i];
                _diskData[i] = null;
                freeBlockIdx++;
            }
        }
    }
}