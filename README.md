# IOT Controller

## Description

IOT Controller est une application de contrôle domotique utilisant .NET MAUI et MQTT pour gérer les appareils connectés. Ce projet implémente des circuits IoT simulés avec Wokwi et ESP32, avec transmission de données via HiveMQ.

## Installation

### Prérequis

- [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/)
- [Visual Studio](https://visualstudio.microsoft.com/)
- [Node.js](https://nodejs.org/)
- [HiveMQ Community Broker](https://www.hivemq.com/products/community/)
- [Wokwi](https://wokwi.com/)

### Étapes d'installation

1. Clonez le dépôt :
    ```bash
    git clone https://github.com/votre-utilisateur/IOT_Controller.git
    cd IOT_Controller
    ```

2. Installez les dépendances pour le projet Node.js :
    ```bash
    cd NodeServer
    npm install
    ```

3. Ouvrez le projet dans Visual Studio :
    ```bash
    start IOT_Controller.sln
    ```

4. Configurez les paramètres dans `appsettings.json` pour votre serveur HiveMQ et autres configurations nécessaires.

5. Exécutez le serveur Node.js :
    ```bash
    node server.js
    ```

6. Lancez l'application .NET MAUI depuis Visual Studio.

## Usage

1. Configurez vos appareils IoT dans Wokwi et connectez-les au serveur MQTT configuré.
2. Utilisez l'application IOT Controller pour surveiller et contrôler vos appareils connectés.

## Contributing

Pour contribuer :

1. Forkez le dépôt.
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/NouvelleFonctionnalite`).
3. Commitez vos modifications (`git commit -m 'Ajout d'une nouvelle fonctionnalité'`).
4. Poussez vers la branche (`git push origin feature/NouvelleFonctionnalite`).
5. Ouvrez une Pull Request.

## Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus d'informations.

## Contact

Pour toute question, veuillez contacter [votre-email@example.com](mailto:votre-email@example.com).
