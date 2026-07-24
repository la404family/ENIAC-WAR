# PREGAME.md — Écrans de Préparation du Jeu (Lobbies / Setup)

> Index des spécifications fonctionnelles et techniques des 3 modes de préparation pour **ENIAC WAR**.

---

## 1. Rôle et Architecture des Écrans de Préparation

Pour offrir une expérience claire et sans ambiguïté au joueur, les écrans de préparation (**Lobbies / Setup**) sont strictement séparés en **trois modes dédiés** depuis le Menu Principal :

1. **[Mode Solo (ENIAC SOLO)](./PREGAME_SOLO.md)** : 
   - Dédié au jeu contre l'Intelligence Artificielle (FSM).
   - Pas de contraintes de réseau, pas de délai, contrôle total sur la configuration des bots et de la carte.

2. **[Mode Hôte LAN (ENIAC HÔTE)](./PREGAME_HOST.md)** :
   - Dédié à la création et l'administration d'un salon multijoueur sur le réseau local.
   - Diffusion UDP, gestion des autorisations/kicks, verrouillage des règles et validation du lancement de l'assaut.

3. **[Mode Client LAN (ENIAC CLIENT)](./PREGAME_CLIENT.md)** :
   - Dédié à la recherche (Server Browser UDP) et à la connexion aux salons LAN existants.
   - Sélection du pseudo/couleur, bascule de l'état **PRÊT** et synchronisation en lecture seule des règles dictées par l'hôte.

---

## 2. Accès direct depuis MenuScreen

Le Menu Principal (`MenuScreen`) propose directement l'accès à chacun des 3 modes de préparation :

- `ENIAC SOLO`  ---> Ouvre `PreGameSoloScreen`
- `ENIAC HOTE`  ---> Ouvre `PreGameHostScreen`
- `ENIAC CLIENT` ---> Ouvre `PreGameClientScreen`
- `OPTIONS`
- `CREDITS`
- `FERMER`

---

## 3. Matrice Comparative des Modes

| Fonctionnalité | ENIAC SOLO | ENIAC HÔTE | ENIAC CLIENT |
|---|---|---|---|
| **Diffusion UDP (Network)** | Non | Oui (Serveur Broadcaster) | Écoute UDP / Server Browser |
| **Gestion des Slots IA** | 1 à 3 IA | Attribution par l'Hôte | Lecture seule |
| **Choix de la Seed & Règles** | Oui | Oui | Lecture seule |
| **Bouton de Lancement** | Instantané (Start) | Soumis à validation PRÊT | Inexistant (Attente Hôte) |
| **Console & Chat LAN** | Journal local | Console d'Administration | Chat Joueur |

---

## 4. Spécifications Détaillées

Veuillez consulter les documents dédiés pour l'implémentation de chaque écran :
- **[`PREGAME_SOLO.md`](./PREGAME_SOLO.md)**
- **[`PREGAME_HOST.md`](./PREGAME_HOST.md)**
- **[`PREGAME_CLIENT.md`](./PREGAME_CLIENT.md)**
