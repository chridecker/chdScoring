using Blazored.Modal;
using Blazored.Modal.Services;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.General.Form;
using chd.UI.Base.Contracts.Enum;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Extensions
{
   public static class ModalExtensions
    {
        public static async Task<EDialogResult> ShowSmallDialog(this IModalHandler modalService, string message, EDialogButtons buttons, RenderFragment? childContent = null)
        {
            var parameter = new ModalParameters()
            {
                {nameof(MessageDialog.Buttons),buttons},
                {nameof(MessageDialog.ChildContent),childContent }
            };
            var modal = modalService.Show<MessageDialog>(message, parameter, new ModalOptions
            {
                DisableBackgroundCancel = true,
                HideCloseButton = true,
                Class = "chd-app-modal-small",
                PositionCustomClass = ""
            });
            ModalResult res = await modal.Result;
            if (res.Cancelled)
            {
                return EDialogResult.None;
            }

            object data = res.Data;
            EDialogResult dialogResult = default(EDialogResult);
            int num;
            if (data is EDialogResult)
            {
                dialogResult = (EDialogResult)data;
                num = 1;
            }
            else
            {
                num = 0;
            }

            if (num == 0)
            {
                throw new Exception($"Ergebnis von {"MessageDialog"} ist ungültig [{res.Cancelled}, {res.Data}]");
            }

            return dialogResult;
        }

        public static async Task<string> ShowSmallInputDialog(this IModalHandler modalService, string message, bool isiOS, string placeHolder = null, RenderFragment? childContent = null)
        {
            var parameter = new ModalParameters()
            {
                {nameof(InputDialog.Placeholder),placeHolder},
                {nameof(InputDialog.ChildContent),childContent },
                {nameof(InputDialog.ConfirmText),"Übernehmen"},
            };
            var modal = modalService.Show<InputDialog>(message, parameter, new ModalOptions
            {
                DisableBackgroundCancel = true,
                HideCloseButton = true,
                Class = $"chd-app-modal-small {(isiOS ? "ios" : "")}",
                PositionCustomClass = "chd-app-modal-position-bottom"
            });
            ModalResult res = await modal.Result;
            if (res.Confirmed && res.Data is string result)
            {
                return result;
            }
            return string.Empty;
        }

        public static async Task<EDialogResult> ShowYesNoDialog(this IModalHandler modalService, string message, bool isiOS, RenderFragment? childContent = null)
        {
            var parameter = new ModalParameters()
            {
                {nameof(MessageDialog.Buttons),EDialogButtons.YesNo},
                {nameof(MessageDialog.ChildContent),childContent }
            };
            var modal = modalService.Show<MessageDialog>(message, parameter, new ModalOptions
            {
                DisableBackgroundCancel = true,
                HideCloseButton = true,
                Class = $"chd-app-modal-small {(isiOS ? "ios" : "")}",
                PositionCustomClass = "chd-app-modal-position-bottom"
            });
            ModalResult res = await modal.Result;
            if (res.Cancelled)
            {
                return EDialogResult.None;
            }

            object data = res.Data;
            EDialogResult dialogResult = default(EDialogResult);
            int num;
            if (data is EDialogResult)
            {
                dialogResult = (EDialogResult)data;
                num = 1;
            }
            else
            {
                num = 0;
            }

            if (num == 0)
            {
                throw new Exception($"Ergebnis von {"MessageDialog"} ist ungültig [{res.Cancelled}, {res.Data}]");
            }

            return dialogResult;
        }
       
        public static async Task<EDialogResult> ShowOkCancelDialog(this IModalHandler modalService, string message, bool isiOS, RenderFragment? childContent = null)
        {
            var parameter = new ModalParameters()
            {
                {nameof(MessageDialog.Buttons),EDialogButtons.OKCancel},
                {nameof(MessageDialog.ChildContent),childContent }
            };
            var modal = modalService.Show<MessageDialog>(message, parameter, new ModalOptions
            {
                DisableBackgroundCancel = true,
                HideCloseButton = true,
                Class = $"chd-app-modal-small {(isiOS ? "ios" : "")}",
                PositionCustomClass = "chd-app-modal-position-bottom"
            });
            ModalResult res = await modal.Result;
            if (res.Cancelled)
            {
                return EDialogResult.None;
            }

            object data = res.Data;
            EDialogResult dialogResult = default(EDialogResult);
            int num;
            if (data is EDialogResult)
            {
                dialogResult = (EDialogResult)data;
                num = 1;
            }
            else
            {
                num = 0;
            }

            if (num == 0)
            {
                throw new Exception($"Ergebnis von {"MessageDialog"} ist ungültig [{res.Cancelled}, {res.Data}]");
            }

            return dialogResult;
        }
    }
}
