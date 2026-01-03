namespace Mymarket.Application.Interfaces;
public interface ITreeEntity<TKey>
{
    TKey Id { get; }
    TKey? ParentId { get; }
}

