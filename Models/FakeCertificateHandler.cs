using UnityEngine.Networking;

namespace Paulov.Tarkov.Minimal.Models;

public class FakeCertificateHandler : CertificateHandler
{
    public override bool ValidateCertificate(byte[] certificateData) => true;
}