namespace Mymarket.Application.Interfaces;

public interface ITreeNode<TNode>
{
    List<TNode>? Children { get; set; }
}