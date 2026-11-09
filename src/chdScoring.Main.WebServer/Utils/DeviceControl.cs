using chd.Base.Hosting.Contracts.Interfaces;
using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Main.WebServer.Modules;
using Microsoft.AspNetCore.SignalR;

namespace chdScoring.Main.WebServer.Utils
{
    public static class DeviceControl
    {
        public static async Task HandleListContextMenu(this IModuleManager moduleManager, DeviceStatusDto item)
        {
            var menu = new ContextMenuStrip();
            var gotToMenu = new ToolStripMenuItem("Go To");
            foreach (var route in ClientRouteConstants.GetConstants())
            {
                gotToMenu.DropDownItems.Add(route.Name, null, async (_, _) =>
                {
                    await SendRedirect(moduleManager, item, route.Value.ToString());
                });
            }
            menu.Items.Add(gotToMenu);

            var autoRedirectMenu = new ToolStripMenuItem("Auto Redirect");
            foreach (var route in ClientRouteConstants.GetConstants())
            {
                autoRedirectMenu.DropDownItems.Add(route.Name, null, async (_, _) =>
                {
                    await SendAutoRedirect(moduleManager, item, route.Value.ToString());
                });
            }
            autoRedirectMenu.DropDownItems.Add("Clear", null, async (_, _) =>
            {
                await SendAutoRedirect(moduleManager, item, string.Empty);
            });

            menu.Items.Add(autoRedirectMenu);

            var setZoomMenu = new ToolStripMenuItem("Zoom");

            foreach (var val in new List<int> { 50, 60, 75, 80, 90, 100, 110, 125, 150, 200 }) // 50 to 200
            {
                setZoomMenu.DropDownItems.Add($"{val}%", null, async (_, _) =>
                {
                    await SetZoom(moduleManager, item, val);
                });
            }

            menu.Items.Add(setZoomMenu);


            menu.Items.Add("Toggle Theme", null, async (_, _) =>
            {
                await SendToggleTheme(moduleManager, item);
            });
            menu.Items.Add("Beep", null, async (_, _) =>
            {
                await SendIdentify(moduleManager, item);
            });

            menu.Items.Add("Restart", null, async (_, _) =>
            {
                await SendShutdown(moduleManager, item, true);
            });



            menu.Show(Cursor.Position);
        }

        private static Task SendToggleTheme(IModuleManager moduleManager, DeviceStatusDto dto)
           => SendDeviceHub(moduleManager, async c =>
           {
               await c.ToggleTheme(dto);
           }, dto);
        private static Task SendIdentify(IModuleManager moduleManager, DeviceStatusDto dto)
            => SendDeviceHub(moduleManager, async c =>
            {
                await c.Identify(dto);
            }, dto);

        private static Task SendShutdown(IModuleManager moduleManager, DeviceStatusDto dto, bool restart)
            => SendDeviceHub(moduleManager, async c =>
            {
                await c.ShutDown(dto, restart);
            }, dto);
        private static Task SendRedirect(IModuleManager moduleManager, DeviceStatusDto dto, string route)
            => SendDeviceHub(moduleManager, async c =>
            {
                await c.NavigateTo(dto, route);
            }, dto);

        private static Task SendAutoRedirect(IModuleManager moduleManager, DeviceStatusDto dto, string route)
            => SendDeviceHub(moduleManager, async c =>
            {
                await c.SetAutoRedirect(dto, route);
            }, dto);

        private static Task SetZoom(IModuleManager moduleManager, DeviceStatusDto dto, int zoom)
            => SendDeviceHub(moduleManager, async c =>
            {
                await c.SetZoom(dto, zoom);
            }, dto);

        private static async Task SendDeviceHub(IModuleManager moduleManager, Func<IDeviceHub, Task> action, DeviceStatusDto dto)
        {
            try
            {
                var hubContext = moduleManager.GetModuleService<IHubContext<DeviceHub, IDeviceHub>>(nameof(ServerModule));
                await action(hubContext.Clients.Client(dto.ConnectionId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
