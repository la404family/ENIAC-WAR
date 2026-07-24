Rôle : Programmeur Interface Utilisateur (UI/UX) C#.

Objectif : Programmer l'interface de contrôle de ENIAC WAR (Electronic Numerical Integrator and Computer) façon écran de commandement militaire rétro.

1. Contrôles 100% Souris :
- Toutes les actions, ordres, développement des unités se font **exclusivement à la souris** via des clics sur la carte et des interactions avec les bouttons.
- Le clavier permet de se déplacer sur la carte z haut s bas d droite q gauche a zoom e dézoom (clavier AZERTY)


2. Layout de l'Écran de Jeu :

**Barre supérieure (Header)** — Bandeau horizontal en haut de l'écran affichant en permanence :
- Le temps du jeu minutes 000, secondes 00 et centieme de seconde 00 séparer par ":" 
- Le score actuel (calculé par les cases conquises, unités abattues, et bonus de capitale détruite).
- Les points d'unités disponibles (ressource pour déployer des unités).

**Écran de début de partie (Lobby/Setup)** :
- Sélection des ressources de base (points d'unités).
- Choix des conditions de victoire : Limite de temps, Objectif de points, ou Domination totale.

**Carte principale** — la carte comporte des lignes topographique comme s'il y avait des plaines et petite montagne de couleur jaune clair et il y a un quadrillage qui divise la carte en 100 sur 100 carré, la carte est divisé en 2250 sur 2250 meme si la fenetre est un 16:9 zoomable

**Panneau inférieur gauche (Info pays + Log)** — Encart semi-transparent avec bordure rétro affichant :
- En-tête : Nom du Pays du joueur, noms du Joueur  et les pays et noms des autres joueurs
- En dessous : **Log d'événements** défilant avec les tags de priorité :
  - `[NORMAL]` — Événements courants 
  - `[IMPORTANT]` — Événements critiques


**Minimap** — Intégrée dans la barre latérale ou dans un coin, permettant de garder une vue d'ensemble de la carte mondiale pendant les zooms. Tient compte du brouillard de guerre.

**Mode Spectateur** :
- Si la capitale d'un joueur est détruite, il passe en mode spectateur : l'UI s'adapte, il voit toute la carte (le brouillard de guerre se dissipe) mais il ne peut plus agir.



