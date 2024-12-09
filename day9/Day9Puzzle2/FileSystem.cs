record DiskBlock(int Position, int Length);

class FileSystem
{
    private List<int?> _diskData = [];
    private List<DiskBlock> _fileBlocks = [];
    private List<DiskBlock> _freeBlocks = [];

    public long Checksum => _diskData.Select((id, i) => (long)i * (id ?? 0)).Sum();

    public FileSystem(string diskMap)
    {
        var fileId = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            var blocks = diskMap[i] & 0xF;
            if (i % 2 == 0)
            {
                _fileBlocks.Add(new(_diskData.Count, blocks));
                _diskData.AddRange(Enumerable.Repeat<int?>(fileId, blocks));
                fileId++;
            }
            else
            {
                _freeBlocks.Add(new(_diskData.Count, blocks));
                _diskData.AddRange(Enumerable.Repeat<int?>(null, blocks));
            }
        }
    }

    public void Defragment()
    {
        for (int i = _fileBlocks.Count - 1; i >= 0; i--)
        {
            var fileBlock = _fileBlocks[i];
            var fileBlockId = _diskData[fileBlock.Position];

            var freeBlockIdx = _freeBlocks.FindIndex(fb => fb.Length >= fileBlock.Length);
            var freeBlock = _freeBlocks.ElementAtOrDefault(freeBlockIdx);

            if (freeBlock == null) continue;
            if (freeBlock.Position > fileBlock.Position) continue;

            for (int j = freeBlock.Position; j < freeBlock.Position + fileBlock.Length; j++)
            {
                _diskData[j] = fileBlockId;
            }

            for (int j = fileBlock.Position; j < fileBlock.Position + fileBlock.Length; j++)
            {
                _diskData[j] = null;
            }

            _freeBlocks[freeBlockIdx] = freeBlock with
            {
                Position = freeBlock.Position + fileBlock.Length,
                Length = freeBlock.Length - fileBlock.Length
            };
        }
    }

    public void PrintDiskMap()
    {
        _diskData.ForEach(id => Console.Write(id != null ? '\u2588' : '\u2592'));
    }
}