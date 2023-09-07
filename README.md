# GameSync

## But
Cette appli web a pour but de faciliter l'organisation de soirées "jeux de société". Le cahier des charges décrivant la problématique en détails se trouve sur le [second repo](https://github.com/PDG2023/GameSync/blob/main/CdC/Cahier%20des%20Charges%20V1.2.pdf)

## Informations générales
Toutes les informations concernant l'app elle-même se situent dans l'onglet [Wiki](https://github.com/PDG2023/GameSync.App/wiki) du projet. S'y trouvent notamment l'architecture des différents environnements, les instructions de lancement en local, processus de travail, etc ...

## Limitations connues
- L'API de [BGG](https://boardgamegeek.com/) utilisée pour récupérer les informations sur les jeux ne fourni pas de système de pagination. Donc les requêtes de recherche sont relativement lentes. 
    - Un système de cache a été mis en place : une fois que la recherche a été faite une fois, cela devient nettement plus rapide.
- Le build du container APP ne passe pas toujours au premier `docker compose up` selon les outils utilisés.
    - Le relancer une seconde fois résolve généralement le problème. Une alternative qui fonctionne à 100% est de lancer la commande depuis un terminal, sans passer par l'assistance d'un IDE.
