# ENIAC WAR — Architecture du Projet

> RTS (Real-Time Strategy) multi LAN · C# / MonoGame

Ce projet est conçu comme un jeu de stratégie en temps réel centré sur la guerre pure (sans gestion diplomatique complexe), avec une direction artistique stricte **Rétro PC / Minitel (écran cathodique noir et vert lumineux)** et une architecture technique (ECS) extrêmement performante.

---

## 1. Documentation de Game Design (GDD)

Les fichiers suivants définissent les règles strictes de conception (à destination des développeurs et des IA) :

- **[`PROMPT.md`](./PROMPT.md)** : Les directives architecturales absolues. Définit l'utilisation de l'ECS (Entity Component System), des Machines à États Finis (FSM), l'interdiction totale des commentaires, les contraintes de mémoire, et la logique du Multijoueur LAN.
- **[`STYLE.md`](./STYLE.md)** : Définit l'esthétique visuelle (Rendu vectoriel pur, brouillard de guerre, expansion territoriale) ainsi que toutes les **statistiques et l'équilibrage des unités** (Pierre-Papier-Ciseaux).
- **[`UI.md`](./UI.md)** : Explique le fonctionnement de l'interface utilisateur, de la caméra (clavier), des contrôles d'unités (souris), et le comportement du Mode Spectateur.
- **[`PREGAME.md`](./PREGAME.md)** : Index des 3 spécifications de préparation (**[`PREGAME_SOLO.md`](./PREGAME_SOLO.md)**, **[`PREGAME_HOST.md`](./PREGAME_HOST.md)**, **[`PREGAME_CLIENT.md`](./PREGAME_CLIENT.md)**).
- **[`SOUNDS.md`](./SOUNDS.md)** : La conception audio. Impose un son 100% procédural (zéro fichier `.wav`), basé sur une priorisation à 3 niveaux pour les événements militaires.

---

## 2. Architecture du Projet (Arborescence)

Le code respecte une organisation strictly modulaire. Aucun asset externe n'est utilisé en dehors d'un unique fichier de police d'écriture `.ttf`.

```text
ENIAC WAR/
├── PROMPT.md                  : Directives architecturales absolues (ECS, FSM, 0 commentaire, LAN).
├── STYLE.md                   : Direction artistique, palette Minitel CRT, stats & équilibrage des unités.
├── UI.md                      : Contrôles souris/clavier, HUD en jeu, minimap et mode spectateur.
├── PREGAME.md                 : Index master des spécifications du lobby.
├── PREGAME_SOLO.md            : Spécifications de préparation pour le mode Solo (vs IA).
├── PREGAME_HOST.md            : Spécifications de préparation pour l'Hôte LAN (Serveur, règles, broadcast).
├── PREGAME_CLIENT.md          : Spécifications de préparation pour le Client LAN (Server Browser, prêts).
├── SOUNDS.md                  : Moteur sonore procédural (0 fichier audio externe).
├── README.md                  : Présentation générale et arborescence du projet.
│
├── Core/
│   ├── Program.cs             : Point d'entrée natif. Intercepte les erreurs critiques pour générer un fichier `crash.log`.
│   ├── Game1.cs               : Classe racine. Initialise la fenêtre, dessine la grille CRT de fond, et délègue la boucle de jeu au ScreenManager.
│   ├── LocalizationManager.cs : Système de traduction modulaire 0 allocation, fusionnant dynamiquement tous les fichiers JSON du dossier Translations au démarrage.
│   └── SettingsManager.cs     : Gestionnaire de paramètres sauvegardés localement (Langue, Résolution, Plein écran).
│
├── Engine/
│   ├── AudioEngine.cs         : Moteur audio procédural. Génère des bips sinusoïdaux en mémoire sans aucun fichier audio (.wav, .mp3).
│   └── Renderer.cs            : Moteur de rendu vectoriel. Trace des primitives (lignes, remplissage) et du texte via un pixel blanc généré en RAM.
│
├── Screens/
│   ├── IScreen.cs             : Interface de contrat universel pour le cycle de vie de toutes les pages (Initialize, Update, Draw).
│   ├── ScreenManager.cs       : Orchestrateur de pages. Gère la page active et exécute les transitions asynchrones en fondu enchaîné.
│   ├── IntroScreen.cs         : Page de démarrage. Séquence d'animation typographique asynchrone façon "Hack Minitel".
│   ├── MenuScreen.cs          : Menu principal (ENIAC SOLO, ENIAC HOTE, ENIAC CLIENT, OPTIONS, CREDITS, FERMER).
│   ├── PreGameSoloScreen.cs   : Écran de préparation Solo (vs IA).
│   ├── PreGameHostScreen.cs   : Écran de préparation Hôte LAN.
│   ├── PreGameClientScreen.cs : Écran de préparation Client LAN.
│   └── OptionsScreen.cs       : Page des réglages avec modale de confirmation sécurisée.
│
├── fonts/
│   └── IBMPlexMono-Regular.ttf  : Fichier de police d'écriture (IBM Plex Mono / Rétro Terminal) utilisé pour le rendu typographique.
│
└── Content/
    └── Translations/          : Dossier des dictionnaires de localisation (EN, FR, ES, DE, IT, PT-BR, TR).
        ├── Global.json        : Textes partagés globalement.
        ├── IntroScreen.json   : Textes de l'écran d'introduction.
        ├── MenuScreen.json    : Textes du menu principal (ENIAC SOLO, ENIAC HOTE, ENIAC CLIENT).
        ├── PreGameScreen.json : Textes des écrans de préparation.
        └── OptionsScreen.json : Textes de l'écran des paramètres.
```
