using Deszz.Undebugger.Model;
using Deszz.Undebugger.UI.Layout;
using Deszz.Undebugger.UI.Windows;
using UnityEngine;

namespace Deszz.Undebugger.UI.Menu
{
    public struct MenuContext
    {
        public UndebuggerConfiguration Configuration;
        internal UndebuggerSettings Settings;
    }

    public class MenuView : MonoBehaviour, IWindowContent, IWindowButtonsContainer
    {
        [SerializeField]
        private GroupButton groupButtonTemplate;
        [SerializeField]
        private RectTransform groupButtonContainer;
        [SerializeField]
        private Transform groupContainer;
        [SerializeField]
        private RectTransform groupButtonsWrapper;
        [SerializeField]
        private RectTransform windowButtonsContainer;

        private MenuModel model;
        private MenuContext context;
        private GroupButton[] groupButtons;
        private GroupView activeGroupView;
        private Window window;

        public void Load(MenuModel model, MenuContext context)
        {
            model.Sort();

            this.model = model;
            this.context = context;

            InitializeGroupButtons();
            SetActiveGroup(0);
        }

        public void Close()
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void SetActiveGroup(int group)
        {
            if (activeGroupView != null)
            {
                DestroyImmediate(activeGroupView.gameObject);
                activeGroupView = null;
            }

            if (group < 0 || group >= context.Settings.GroupTemplates.Length)
            {
                for (int i = 0; i < groupButtons.Length; ++i)
                {
                    groupButtons[i].SetSelected(false);
                }

                return;
            }

            var view = Instantiate(context.Settings.GroupTemplates[group], groupContainer.transform);
            view.Load(model, context);

            activeGroupView = view;

            for (int i = 0; i < groupButtons.Length; ++i)
            {
                groupButtons[i].SetSelected(i == group);
            }

            LayoutUtility.SetLayoutDirty(groupContainer, LayoutDirtyFlag.All);
        }

        private void InitializeGroupButtons()
        {
            if (groupButtons != null)
            {
                for (int i = 0; i < groupButtons.Length; ++i)
                {
                    if (groupButtons[i] != null)
                    {
                        groupButtons[i].Clicked -= GroupButtonClickedHandler;
                        GameObject.Destroy(groupButtons[i].gameObject);
                    }
                }
            }

            if (groupButtons == null || groupButtons.Length != context.Settings.GroupTemplates.Length)
            {
                groupButtons = new GroupButton[context.Settings.GroupTemplates.Length];
            }

            for (int i = 0; i < context.Settings.GroupTemplates.Length; ++i)
            {
                groupButtons[i] = Instantiate(groupButtonTemplate, groupButtonContainer);
                groupButtons[i].Init(i, context.Settings.GroupTemplates[i].GroupName);
                groupButtons[i].Clicked += GroupButtonClickedHandler;
            }

            LayoutUtility.SetLayoutDirty(groupButtonContainer, LayoutDirtyFlag.All);
        }

        private void GroupButtonClickedHandler(int index)
        {
            SetActiveGroup(index);
        }

        public void AddingToWindow(Window window)
        {
            this.window = window;
        }

        public void RemovedFromWindow(Window window)
        {
            this.window = null;
        }

        public void AttachWindowButtons(WindowControlButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].transform.SetParent(windowButtonsContainer);
            }

            LayoutUtility.SetLayoutDirtyAndForceUpdate(windowButtonsContainer, LayoutDirtyFlag.All);
            groupButtonsWrapper.offsetMin = new Vector2(windowButtonsContainer.rect.width, groupButtonsWrapper.offsetMin.y);
        }

        public void DetachWindowButtons()
        {
            for (int i = windowButtonsContainer.childCount - 1; i >= 0; --i)
            {
                DestroyImmediate(windowButtonsContainer.GetChild(i).gameObject);
            }

            LayoutUtility.SetLayoutDirtyAndForceUpdate(windowButtonsContainer, LayoutDirtyFlag.All);
            groupButtonsWrapper.offsetMin = new Vector2(windowButtonsContainer.rect.width, groupButtonsWrapper.offsetMin.y);
        }
    }
}
