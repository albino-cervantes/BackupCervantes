using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
    private ServiceProcessInstaller _process;
    private ServiceInstaller _service;

    public ProjectInstaller()
    {
        _process = new ServiceProcessInstaller
        {
            Account = ServiceAccount.LocalSystem
        };

        _service = new ServiceInstaller
        {
            ServiceName = "BackupAutomaticoCervantes",
            DisplayName = "Backup Automático Cervantes",
            StartType = ServiceStartMode.Automatic
        };

        Installers.Add(_process);
        Installers.Add(_service);
    }
}
