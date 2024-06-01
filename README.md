# SolutionIOT(IOT-IOTServer-ControllerApp)

## Description

SolutionIOT(IOT-IOTServer-ControllerApp) est une solution domotique utilisant .NET MAUI pour la creation de l'application de control et MQTT pour gérer les appareils connectés ainsi que NodeJS pour la structuration des donnees. Ce projet implémente des circuits IoT simulés avec Wokwi et ESP32, avec transmission de données via HiveMQ.

## Installation

### Prérequis

- [Visual Studio](https://visualstudio.microsoft.com/)
- [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/)
- [Node.js](https://nodejs.org/)
- [HiveMQ Community Broker](https://github.com/hivemq/hivemq-community-edition?tab=readme-ov-file)
- [HiveMQ Cloud](https://www.hivemq.com/products/mqtt-cloud-broker/)
- [Wokwi](https://wokwi.com/)

### Étapes d'installation

1. Clonez le dépôt :
    ```bash
    git clone https://github.com/TatumLn/Solution-IOT-IOT-Server-ControllerApp.git
    cd Solution-IOT-IOT-Server-ControllerApp
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

4. Configurez les paramètres dans `conf/conf.xml` pour votre serveur HiveMQ et autres configurations nécessaires.

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

## Contact

Pour toute question, veuillez contacter(mailto:ZAFINIAINAhermaprosper@gmail.com).
