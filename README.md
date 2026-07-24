# ENIAC WAR — Architecture du Projet

> RTS (Real-Time Strategy) multi LAN · C# / MonoGame

Ce projet est conçu comme un jeu de stratégie en temps réel centré sur la guerre pure (sans gestion diplomatique complexe), avec une direction artistique stricte **Rétro PC / Minitel (écran cathodique noir et vert lumineux)** et une architecture technique (ECS) extrêmement performante.

---

## 1. Documentation de Game Design (GDD)

Les fichiers suivants définissent les règles strictes de conception (à destination des développeurs et des IA) :

- **[`PROMPT.md`](./PROMPT.md)** : Les directives architecturales absolues. Définit l'utilisation de l'ECS (Entity Component System), des Machines à États Finis (FSM), l'interdiction totale des commentaires, les contraintes de mémoire, et la logique du Multijoueur LAN.
- **[`STYLE.md`](./STYLE.md)** : Définit l'esthétique visuelle (Rendu vectoriel pur, brouillard de guerre, expansion territoriale) ainsi que toutes les **statistiques et l'équilibrage des unités** (Pierre-Papier-Ciseaux).
- **[`UI.md`](./UI.md)** : Explique le fonctionnement de l'interface utilisateur, de la caméra (clavier), des contrôles d'unités (souris), le lobby de départ, et le comportement du Mode Spectateur.
- **[`SOUNDS.md`](./SOUNDS.md)** : La conception audio. Impose un son 100% procédural (zéro fichier `.wav`), basé sur une priorisation à 3 niveaux pour les événements militaires.

---

## 2. Architecture du Projet (Arborescence)

Le code respecte une organisation strictement modulaire. Aucun asset externe n'est utilisé en dehors d'un unique fichier de police d'écriture `.ttf`.

```text
ENIAC WAR/
├── Core/
│   ├── Program.cs         : Point d'entrée natif de l'application MonoGame.
│   └── Game1.cs           : Classe racine. Initialise la fenêtre 720p, dessine la grille CRT de fond, et délègue la boucle de jeu au ScreenManager.
│
├── Engine/
│   ├── AudioEngine.cs     : Moteur audio procédural. Génère des bips sinusoïdaux en mémoire sans aucun fichier audio (.wav, .mp3).
│   └── Renderer.cs        : Moteur de rendu vectoriel. Trace des primitives (lignes, remplissage) et du texte via un pixel blanc généré en RAM.
│
├── Screens/
│   ├── IScreen.cs         : Interface de contrat universel pour le cycle de vie de toutes les pages (Initialize, Update, Draw).
│   ├── ScreenManager.cs   : Orchestrateur de pages. Gère la page active et exécute les transitions asynchrones en fondu enchaîné.
│   ├── IntroScreen.cs     : Page de démarrage. Séquence d'animation typographique asynchrone façon "Hack Minitel".
│   └── MenuScreen.cs      : Page du menu principal. Navigation au clavier avec curseur bloc (NOUVELLE CAMPAGNE, OPTIONS, FERMER).
│
└── Content/ & fonts/
    └── JetBrainsMono      : L'unique fichier de police utilisé pour le rendu typographique (la seule exception tolérée à la règle "Zéro Asset").
```
