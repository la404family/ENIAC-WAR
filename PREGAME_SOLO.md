# PREGAME_SOLO.md — Écran de Préparation : Mode Solo (ENIAC SOLO)

> Spécifications de l'interface de préparation pour les parties en Solo contre l'Intelligence Artificielle.

---

## 1. Rôle & Objectif

L'écran **PREGAME_SOLO** permet à un joueur unique de paramétrer sa confrontation tactique contre 1 à 3 adversaires contrôlés par des Machines à États Finis (FSM - IA).

L'interface se veut fluide, instantanée et débarrassée des contraintes réseau (pas d'attente d'autres joueurs, pas d'indicateur PRÊT requis).

---

## 2. Configuration des Slots Joueurs (Mode Solo)

Le joueur humain est automatiquement positionné sur le **Slot 1 (Commandant principal)**.

- **Slot 1 (Humain Local)** :
  - Nom du Commandant (modifiable, ex: `COMMANDANT`).
  - Sélection de la couleur tactique (`VERT`, `BLEU`, `ROUGE`, `CYAN`).
  - Emplacement de Capitale (Aléatoire ou quadrant défini).
- **Slots 2 à 4 (Adversaires IA)** :
  - Type de Slot : `IA FACILE`, `IA MOYEN`, `IA DIFFICILE`, ou `FERMÉ`.
  - Profil d'IA :
    - *Facile* : Expansion lente, attaque uniquement si provoquée.
    - *Moyen* : Expansion équilibrée, production mixte d'infanterie/chars.
    - *Difficile* : Agressivité maximale, harcèlement aérien et contournements.
  - Attribution automatique des couleurs restantes.

---

## 3. Paramètres de la Partie Solo

L'utilisateur maîtrise intégralement la configuration environnementale :

- **Graine de Carte (Seed)** : Champ texte ou génération aléatoire.
- **Ressources Initiales** : `50 pts`, `100 pts` (défaut), `200 pts`.
- **Limite de Population** : `50`, `100`, `150` unités max par belligérant.
- **Brouillard de Guerre** : `ACTIVÉ` (défaut) / `DÉSACTIVÉ`.
- **Conditions de Victoire** : `DOMINATION TOTALE`, `CHRONO (10/20/30 min)`, `SCORE CIBLE`.

---

## 4. Layout Minitel CRT (Vectoriel Pur)

```text
+-----------------------------------------------------------------------------------+
|  ENIAC SOLO — CONFIGURATION DE L'AFFRONTEMENT IA                                 |
+------------------------------------------------------+----------------------------+
| ADVERSAIRES                                          | PARAMÈTRES DU MONDE        |
|                                                      |                            |
| [1] JOUEUR (VOUS) | CMD_ALPHA   | [VERT]  | OK       | SEED      : 49201948       |
| [2] IA (MOYEN)    | BOT_ALPHA   | [BLEU]  | OK       | CARTE     : 100x100 CASES  |
| [3] IA (DIFFICILE)| BOT_BETA    | [ROUGE] | OK       | PTS DÉPART: 100 PTS        |
| [4] FERMÉ         | ---         | ---     | ---      | POP MAX   : 150 UNITES     |
|                                                      | VICTOIRE  : DOMINATION     |
+------------------------------------------------------+----------------------------+
| LOG TACTIQUE PREGAME                                 | APERÇU CARTE PROCÉDURALE   |
|                                                      |                            |
| [INFO] 2 ADVERSAIRES IA CONFIGURÉS.                  |       /\   /\  .           |
| [INFO] TOUS LES SYSTÈMES PRÊTS POUR SIMULATION.      |      /  \ /  \             |
|                                                      |     (  TOPOGRAPHIE  )      |
+------------------------------------------------------+----------------------------+
| [RETOUR MENU]                                            [LANCER L'ASSAUT (START)]|
+-----------------------------------------------------------------------------------+
```

---

## 5. Démarrage de la Partie

Dès que l'utilisateur valide via **LANCER L'ASSAUT**, la transition asynchrone est immédiatement initiée vers `GameScreen` sans délai réseau.
