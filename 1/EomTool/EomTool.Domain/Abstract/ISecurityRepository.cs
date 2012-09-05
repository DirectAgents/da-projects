using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface ISecurityRepository
    {
        Group WindowsIdentityGroup(string windowsIdentity);
    }
}
