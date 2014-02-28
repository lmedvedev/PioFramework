using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Windows.Forms;

namespace BOF
{
    public delegate void NodeEventDelegate(object sender, NodeEventArgs e);

    public interface ICtlTree
    {
        event NodeEventDelegate NodeChanged;

        event NodeEventDelegate AskNewNode;

        //void Load(ITree setTree);
        void Select(object node);
    }
    public class NodeEventArgs : EventArgs
    {
        //public CardBaseTreeDat TreeNodeDat
        //{
        //    get
        //    {
        //        if (Node != null && Node.Tag is CardBaseTreeDat)
        //            return (CardBaseTreeDat)Node.Tag;
        //        else
        //            return null;
        //    }
        //}
        public TreeNode Node = null;
        public NodeEventArgs(TreeNode treeNode)
        {

            Node = treeNode;
        }
    }
}
