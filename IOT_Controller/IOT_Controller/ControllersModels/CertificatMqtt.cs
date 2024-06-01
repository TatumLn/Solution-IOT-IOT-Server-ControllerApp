using MQTTnet.Client;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class CertificatMqtt : IMqttClientCertificatesProvider
{
    private readonly string? _caCertPath;
    private readonly string? _clientCertPath;
    private readonly string? _clientCertPassword;

    public CertificatMqtt()
    {
    }

    public CertificatMqtt(string? caCertPath, string? clientCertPath, string? clientCertPassword)
    {
        _caCertPath = caCertPath;
        _clientCertPath = clientCertPath;
        _clientCertPassword = clientCertPassword;
    }

    public IEnumerable<X509Certificate> GetCertificates()
    {
        var certificates = new List<X509Certificate>();

        if (!string.IsNullOrEmpty(_caCertPath))
        {
            certificates.Add(new X509Certificate2(_caCertPath));
        }

        if (!string.IsNullOrEmpty(_clientCertPath))
        {
            certificates.Add(new X509Certificate2(_clientCertPath, _clientCertPassword));
        }

        return certificates;
    }

    X509CertificateCollection IMqttClientCertificatesProvider.GetCertificates()
    {
        throw new NotImplementedException();
    }
}
