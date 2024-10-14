﻿using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using Microsoft.JSInterop;

namespace chdScoring.App.Services
{
    public class UiHandler : BaseUIComponentHandler
    {
        public UiHandler(IJSRuntime jSRuntime, IDefaultIconService defaultIconService) : base(jSRuntime, defaultIconService)
        {
        }
    }
}
