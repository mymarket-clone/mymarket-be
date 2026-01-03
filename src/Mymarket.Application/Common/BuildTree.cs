using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Common;

public static class TreeBuilder
{
    public static IReadOnlyList<TNode> Build<TEntity, TNode>(
        IEnumerable<TEntity> entities,
        IDictionary<int, TNode> nodes,
        Func<TEntity, int> idSelector,
        Func<TEntity, int?> parentIdSelector)
        where TNode : ITreeNode<TNode>
    {
        var roots = new List<TNode>();

        foreach (var entity in entities)
        {
            var id = idSelector(entity);
            var parentId = parentIdSelector(entity);
            var node = nodes[id];

            if (parentId is null)
            {
                roots.Add(node);
                continue;
            }

            var parent = nodes[parentId.Value];
            parent.Children ??= [];
            parent.Children.Add(node);
        }

        return roots;
    }
}


