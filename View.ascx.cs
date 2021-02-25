/*
' Copyright (c) 2021  Mohit
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DNNTreeView.Modules.DNNTreeView
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from DNNTreeViewModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : DNNTreeViewModuleBase, IActionable
    {
        private bool ShowHiddenTabs = false;
        private bool ExpandAllTabs = false;
        private bool ShowExpandCollapse = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    if (Settings.Contains("showhiddentab"))
                    {
                        ShowHiddenTabs = Convert.ToBoolean(Settings["showhiddentab"]);
                    }
                    if (Settings.Contains("expandalltabs"))
                    {
                        ExpandAllTabs = Convert.ToBoolean(Settings["expandalltabs"]);
                    }
                    if (Settings.Contains("showexpandcollapse"))
                    {
                        ShowExpandCollapse = Convert.ToBoolean(Settings["showexpandcollapse"]);
                    }

                    List<TabInfo> tabInfoList = TabController.GetTabsByParent(this.TabId, this.PortalId);
                    foreach (TabInfo tab in tabInfoList)
                    {
                        if (!tab.IsDeleted && (ShowHiddenTabs ? true : tab.IsVisible))
                        {
                            if (tab.HasChildren)
                            {
                                TreeView1.Nodes.Add(getNodeWithChildNodeIfAny(tab));
                            }
                            else
                            {
                                TreeView1.Nodes.Add(getTreeNode(tab));
                            }
                        }
                    }

                    if (Settings.Contains("showlines"))
                    {
                        TreeView1.ShowLines = Convert.ToBoolean(Settings["showlines"]);
                    }

                    // Expand/Collapse Nodes
                    if (ShowExpandCollapse)
                    {
                        TreeView1.ShowExpandCollapse = true;
                        if (ExpandAllTabs)
                        {
                            TreeView1.ExpandAll();
                        }
                        else
                        {
                            TreeView1.CollapseAll();
                        }
                    }
                    else
                    {
                        TreeView1.ShowExpandCollapse = false;
                        TreeView1.ExpandAll();
                    }

                    TreeView1.CssClass = "TreeViewStyle";
                    TreeView1.ParentNodeStyle.CssClass = "ParentNodeStyle";
                    TreeView1.LeafNodeStyle.CssClass = "LeafNodeStyle";
                    TreeView1.HoverNodeStyle.CssClass = "HoverNodeStyle";
                    TreeView1.SelectedNodeStyle.CssClass = "SelectedNodeStyle";
                    TreeView1.RootNodeStyle.CssClass = "RootNodeStyle";
                }

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        private TreeNode getNodeWithChildNodeIfAny(TabInfo tab)
        {
            var treeNode = getTreeNode(tab);

            if (!tab.HasChildren)
            {
                return treeNode;
            }
            else
            {
                List<TabInfo> childTabs = TabController.GetTabsByParent(tab.TabID, this.PortalId);
                foreach (TabInfo childTab in childTabs)
                {
                    if (!childTab.IsDeleted && (ShowHiddenTabs ? true : childTab.IsVisible))
                    {
                        if (childTab.HasChildren)
                        {
                            treeNode.ChildNodes.Add(getNodeWithChildNodeIfAny(childTab));
                        }
                        else
                        {
                            treeNode.ChildNodes.Add(getTreeNode(childTab));
                        }
                    }
                }
            }
            return treeNode;
        }

        private TreeNode getTreeNode(TabInfo tab)
        {
            var treeNode = new TreeNode();
            treeNode.Text = tab.TabName;
            treeNode.NavigateUrl = tab.FullUrl;
            treeNode.Target = "_blank";
            if (tab.DisableLink)
            {
                treeNode.SelectAction = TreeNodeSelectAction.None;
            }
            return treeNode;
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}