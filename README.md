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

## 2. Code Source (Moteur MonoGame)

Le code est structuré de manière modulaire, sans aucun asset externe hormis une police d'écriture `.ttf` (zéro image, zéro fichier audio).

- **[`Program.cs`](./Program.cs)** : Le point d'entrée classique de l'application C# MonoGame.
- **[`Game1.cs`](./Game1.cs)** : La boucle de jeu principale. Il gère l'initialisation de la fenêtre (720p, centrée), le rendu du quadrillage cathodique (CRT) et coordonne l'animation d'introduction façon Minitel (frappe de texte asynchrone avec sons procéduraux et curseurs blocs clignotants).
- **[`Renderer.cs`](./Renderer.cs)** : Un mini-moteur de rendu vectoriel fait maison. Il trace des lignes et des géométries à l'écran grâce à un simple pixel blanc généré en mémoire, respectant la règle du "zéro texture externe".
- **`fonts/` et `Content/`** : Contient la police `JetBrainsMono` utilisée pour afficher les textes de l'UI (le seul "asset externe" toléré).
- **[`AudioEngine.cs`](./AudioEngine.cs)** : Un synthétiseur audio procédural. Il génère des ondes sonores (ex: des "bips" sinusoïdaux) directement dans des tampons mémoire, éliminant le besoin de charger des fichiers `.wav` ou `.mp3`.
