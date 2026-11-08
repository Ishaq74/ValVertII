# VAL VERT II
## Manuel maître consolidé de conception, d'architecture et de production

**Édition consolidée 1.0 — 7 novembre 2026**  
**Moteur de référence : Unity 6.6 LTS — projet 3D URP — C# — PC en première cible**

> Ce manuel fusionne les deux documents techniques joints et l'extension détaillée fournie dans la conversation. Il constitue une spécification de production et un guide de réalisation. Il ne prétend pas qu'un projet Unity, des scripts ou des environnements ont déjà été créés, compilés ou validés.

---

# Comment utiliser ce manuel

Ce document est la référence unique du projet. Les anciennes versions ne doivent plus être suivies séparément lorsqu'elles se contredisent. Les décisions ci-dessous remplacent les intitulés et systèmes hérités du tutoriel générique.

- **À construire** signifie que la fonctionnalité appartient au périmètre du jeu.
- **Option de production** signifie qu'elle ne sera ajoutée que si les mesures ou le design la justifient.
- **Hors périmètre** signifie qu'elle ne doit pas être importée du tutoriel générique.
- **Jalon validé** ne peut être coché qu'après intégration, test dans Unity et conservation d'une preuve de test. La présence d'un script dans le dossier ne suffit pas.

Le développement doit avancer par tranches verticales jouables : un petit morceau du monde, un personnage, une interaction, une sauvegarde et un test complet avant d'étendre le contenu.

# SOMMAIRE GÉNÉRAL

1. Vision, identité et règles de conception
2. Cahier des charges fonctionnel et périmètre
3. Socle technique et versions
4. Organisation du dépôt et des dossiers Unity
5. Architecture applicative, démarrage et scènes
6. Données, identifiants et événements
7. Entrées et gestion des modes de contrôle
8. Personnage joueur, déplacement et posture
9. Caméras de jeu et changement de vue
10. Mode développeur, vol libre et inspection
11. Personnages jouables et changement de protagoniste
12. Interactions et objets du quotidien
13. Téléphone, messages, appels et agenda
14. Dialogues, scènes et orchestration narrative
15. Relations, mémoire et circulation de l'information
16. Habitants, routines et simulation hybride
17. Navigation, foule et activités sociales
18. Reconstruction de Seynod et du bois du Val Vert
19. Secteurs, chargement et streaming
20. Intérieurs, portes et mobilier utilisable
21. Temps du monde, météo et saisons
22. Inventaire quotidien, objets et activités
23. Audio, rendu, éclairage et animations
24. Interface, accessibilité et localisation
25. Sauvegardes, restauration et migrations
26. Outils Unity et chaîne de production
27. Tests, performance et budgets
28. Builds, sécurité et livraison
29. Ordre de réalisation et jalons de validation
30. Arborescence cible, matrice de traçabilité et décisions ouvertes

---

# TOME 0 — IDENTITÉ DU JEU ET PRINCIPES DIRECTEURS

## 1. Vision de Val Vert II

Val Vert II est une simulation narrative contemporaine en monde ouvert située à Seynod, dans l'agglomération annécienne. Le joueur traverse des lieux de vie reconnaissables : le bois du Val Vert, ses chemins, ses abords, les espaces verts, les immeubles, les rues, les logements, les établissements scolaires, les commerces et les espaces publics retenus pour le scénario.

Le principe central n'est pas de sauver le monde ni d'accumuler une puissance. Il est de vivre une situation humaine dans un environnement qui continue d'exister lorsque le joueur regarde ailleurs. Les personnes ont leurs horaires, leurs habitudes, leurs relations, leurs obligations, leurs interprétations et leurs souvenirs. Une action peut avoir une conséquence immédiate, différée, indirecte ou aucune conséquence notable.

Le monde ne doit pas donner l'impression que chaque habitant attend le joueur. Les personnages secondaires doivent pouvoir poursuivre une activité, changer de destination, discuter entre eux, manquer un rendez-vous, rentrer chez eux ou réagir à une situation sans que le joueur soit présent.

## 2. Les cinq piliers non négociables

1. **Lieu crédible** — Seynod et le bois du Val Vert doivent être reconnaissables par leur organisation spatiale, leurs cheminements et leurs points de repère, dans les limites des références et droits d'utilisation disponibles.
2. **Présence humaine** — les personnages ne sont pas des distributeurs de répliques. Ils ont un contexte, des activités, des relations et des limites de connaissance.
3. **Liberté quotidienne** — marcher, regarder, discuter, rentrer, attendre, s'asseoir, utiliser un objet, consulter son téléphone et choisir de ne pas agir sont des actions importantes.
4. **Narration systémique** — les choix modifient des états du monde et des relations sans afficher une note morale ou une jauge de réputation façon RPG.
5. **Continuité** — le temps, les événements, les personnages, les objets importants et les secteurs retrouvent un état cohérent après chargement et sauvegarde.

## 3. Personnages jouables initiaux

### 3.1 Karim

Karim a 17 ans. Son parcours donne accès à la vie adolescente, au quartier, à la famille, aux amis, aux rencontres, aux obligations et aux choix personnels. Le début de son histoire prend place autour du bois du Val Vert. Le téléphone et les interactions familiales ont un rôle narratif important.

### 3.2 La professeure

Son parcours explore le quotidien professionnel, la gestion d'une classe, les relations avec les élèves et les collègues, les responsabilités, la fatigue et la vie personnelle. Elle ne doit pas être réduite à une simple fonction dans l'histoire de Karim.

### 3.3 Le père

Son parcours explore le travail, les contraintes financières et familiales, les tâches quotidiennes, les préoccupations matérielles et la relation avec son fils.

Les trois parcours ont des états distincts mais partagent un monde, une horloge, des événements et des relations. Un événement dans un parcours ne se propage pas automatiquement aux autres : il faut un mécanisme crédible d'observation, de conversation, d'appel, de message ou de rumeur.

## 4. Ce que Val Vert II n'est pas

Les éléments suivants issus du tutoriel technique générique sont retirés :

- Combat, armes, dégâts, points de vie, mort de joueur ou d'ennemi.
- Ennemis, patrouilles hostiles, poursuite/attaque/fuite de type combat.
- Projectiles, butin de combat, équipement offensif et récompenses de combat.
- Quêtes de type « tuer / collecter pour obtenir une récompense » comme structure centrale.
- Score, expérience, niveaux, arbre de compétences et jauges de réputation visibles.
- Vol libre comme pouvoir narratif de Karim.

Les briques techniques réutilisables (événements, pooling, sauvegarde, ECS, streaming, outils, profiling et tests) sont conservées uniquement lorsqu'elles servent le monde social et quotidien.

## 5. Échelle d'ambition et limites de production

L'ambition est d'obtenir une ville dense, cohérente et libre à explorer. « Aussi complet que GTA » est un repère d'ambition en matière de liberté et de cohérence, pas une promesse de reproduire la taille d'une production AAA. La priorité est la qualité d'un périmètre jouable, puis son extension mesurée.

Le projet distinguera toujours :

- **Décor visible** : modèle extérieur, collision simplifiée ou proxy.
- **Lieu traversable** : chemin ou espace effectivement accessible.
- **Bâtiment accessible** : entrée et sortie fonctionnelles.
- **Intérieur jouable** : espace meublé, navigable, interactif et persistant.
- **Lieu narratif majeur** : espace dont la topologie, les objets et les interactions sont spécifiquement conçus et testés.

Tous les bâtiments ne sont pas obligatoirement accessibles. Les lieux centraux du récit bénéficient d'un niveau de détail supérieur.

---

# TOME I — CAHIER DES CHARGES FONCTIONNEL

## 6. Monde et liberté de déplacement

Le joueur doit pouvoir marcher, courir, regarder autour de lui, monter et descendre des escaliers, franchir les portes prévues, entrer dans les bâtiments accessibles, s'asseoir, se relever, manipuler des objets, consulter son téléphone et parler aux habitants.

La première cible de déplacement est le déplacement à pied. Le vélo, les transports publics et les véhicules éventuels sont des extensions séparées. Ils ne doivent pas être ajoutés avant que le déplacement piéton, les collisions, les accès et les interactions soient stables.

## 7. Fonctions du joueur

- Déplacement clavier/souris et manette.
- Première personne comme vue immersive de référence.
- Troisième personne optionnelle, activable si la présentation du corps et les animations sont prêtes.
- Caméras cinématiques contrôlées par la narration.
- Interactions contextuelles.
- Téléphone diégétique.
- Assise, lever, attente et activités simples.
- Changement de personnage uniquement selon les règles du parcours en mode normal.
- Réglages de confort : sensibilité, inversion verticale, champ de vision, vibrations, mouvements de caméra, sous-titres et volumes.

## 8. Téléphone

Fonctions initiales : appels entrants/sortants, messages, historique, agenda et notifications. La carte, la galerie et les informations contextuelles peuvent venir ensuite. L'interface doit donner l'impression d'un téléphone appartenant au personnage, mais il n'est pas nécessaire de simuler un système d'exploitation complet.

Le téléphone respecte le contexte : une action peut être indisponible pendant une scène, une interaction prioritaire ou une animation nécessitant les deux mains. Les appels manqués et les messages non lus peuvent devenir des faits narratifs.

## 9. Interactions quotidiennes

Les catégories comprennent : portes et fenêtres, mobilier, objets personnels, téléphone, documents, personnes, activités, lieux et déclencheurs narratifs. Une interaction est disponible seulement si l'objet, le personnage, la distance, le contexte, l'état de la scène et les conditions narratives l'autorisent.

Exemples : ouvrir/fermer une porte, s'asseoir sur un banc, lire un document, prendre un objet, saluer, discuter, demander une information, attendre, regarder un élément ou utiliser une table.

## 10. Population et simulation

Les habitants importants possèdent une identité persistante, un domicile ou point d'attache, des routines, des destinations, des relations, des préférences, des souvenirs et des disponibilités. Les habitants secondaires peuvent être simulés plus légèrement. Une personne éloignée n'a pas besoin d'un modèle 3D actif pour conserver une activité logique.

## 11. Narration et conséquences

Les situations sont des ensembles de conditions, d'événements, de réactions et de conséquences potentielles. Le scénario ne doit pas être une liste rigide de missions obligatoires. Le joueur doit parfois pouvoir poursuivre sa route, reporter une action, manquer un événement ou ne pas recevoir une information.

## 12. Temps et persistance

L'horloge du monde contrôle les horaires, rendez-vous, activités, ouvertures, éclairages et événements. La sauvegarde conserve les états importants, pas chaque image ou chaque micro-mouvement.

---

# TOME II — SOCLE TECHNIQUE ET ARCHITECTURE UNITY

## 13. Version moteur et stratégie de dépendances

### 13.1 Choix de référence

Le projet de référence est **Unity 6.6 LTS**, branche `6000.3.x`, avec URP. Ce choix privilégie une branche LTS adaptée à une production qui doit stabiliser son environnement. Au 7 novembre 2026, Unity annonce le support d'Unity 6.6 LTS jusqu'en décembre 2027. Cela ne signifie pas que 6.6 est la version Update la plus récente à cette date ; c'est le choix LTS volontaire du projet.

### 13.2 Packages de référence

| Domaine | Choix | Règle |
|---|---|---|
| Rendu | URP livré avec l'éditeur | Package core : conserver la version correspondant à 6000.3. |
| Entrées | Input System 1.20.1 | Version publiée pour Unity 6000.3 au moment de la vérification. |
| Caméra | Cinemachine 3.1.7 | Version publiée pour Unity 6000.3 au moment de la vérification. |
| Simulation | Entities 1.5.0 | Version publiée pour Unity 6000.3 au moment de la vérification. |
| Population visuelle ECS | Entities Graphics | Installer la version déclarée compatible par Package Manager. |
| Optimisation ECS | Burst, Collections, Mathematics | Installer et verrouiller les versions compatibles proposées pour 6000.3. |
| Navigation | AI Navigation | Installer la version publiée compatible avec l'éditeur. |
| UI | UI Toolkit et/ou uGUI/TMP | Choix selon le besoin de chaque écran ; pas deux systèmes mélangés dans un même widget sans raison. |
| Chargement avancé | Addressables | Optionnel ; ne pas l'introduire avant d'avoir un besoin identifié. |
| Tests | Unity Test Framework | Version compatible avec l'éditeur. |
| Localisation | Unity Localization | À intégrer lorsque les langues et les volumes de texte sont cadrés. |

Les versions exactes d'Entities Graphics, Burst, Collections, Mathematics, AI Navigation et des autres dépendances non listées ici ne doivent pas être inventées. Il faut sélectionner la version publiée compatible dans le Package Manager, puis committer `Packages/manifest.json` et `Packages/packages-lock.json`.

### 13.3 Verrouillage

- Ne pas recopier URP 14, Cinemachine 2.x ou Input System 1.7 depuis des tutoriels Unity 2022.
- Ne pas modifier manuellement `packages-lock.json`.
- Ne pas mettre à jour plusieurs familles de packages au milieu d'un sprint de contenu.
- Toute mise à jour doit passer par une branche dédiée, un build propre et une suite de tests.
- Garder une note de version du moteur, du système d'exploitation, du backend graphique et des versions de packages.

## 14. Rôle des technologies

- **URP** : rendu des environnements, matériaux, éclairage, ombres et post-traitement.
- **Input System** : actions indépendantes du périphérique.
- **Cinemachine 3** : pilotage des caméras de jeu et des transitions.
- **CharacterController** : locomotion du personnage principal, si le comportement reste adapté ; pas de déplacement du personnage par téléportation de Transform à chaque frame.
- **AI Navigation** : navigation des habitants proches représentés par des GameObjects.
- **Entities/ECS** : données et calculs répétitifs de grande population lorsque les mesures le justifient.
- **Burst/Collections/Mathematics** : optimisation des traitements compatibles.
- **JSON versionné** : persistance des états de partie et données narratives.
- **Scènes additives** : secteurs GameObject et intérieurs.
- **SubScenes** : contenu ECS authoré/baké, sans confondre leur cycle de vie avec celui des scènes classiques.
- **Git** : historique et travail reproductible.

## 15. Architecture en couches

### 15.1 Infrastructure applicative

Durée de vie : toute l'exécution de l'application. Elle porte la configuration, les services persistants, le chargement, les logs, les sauvegardes et les transitions vers menu/monde.

### 15.2 Session de monde

Durée de vie : une partie chargée. Elle porte l'horloge, le personnage actif, la simulation, les événements de monde, les secteurs et l'état de la partie.

### 15.3 Gameplay et narration

Elle porte les personnages, interactions, relations, dialogues, activités, téléphone, horaires et mémoire.

### 15.4 Présentation

Elle porte les caméras, animations, audio, UI, effets et présentation visuelle. La présentation consomme des données et événements ; elle ne décide pas de la vérité narrative.

### 15.5 Règles de dépendance

- Core ne référence aucun module de gameplay.
- Save dépend de DTO et de contrats de persistance, jamais de composants visuels.
- Narrative ne dépend pas d'un Animator ou d'une caméra.
- L'interface ne modifie pas directement les relations : elle émet une commande.
- ECS ne doit pas dépendre des GameObjects de présentation.
- Un prefab visuel peut être détruit et recréé sans supprimer l'identité logique d'un habitant.
- Les dépendances sont injectées dans un point de composition explicite ; éviter les singletons globaux non contrôlés.

## 16. Structure de dépôt et versionnement

À la racine du dépôt :

```text
ValVertII/
├── Assets/
│   └── _ValVertII/
├── Packages/
│   ├── manifest.json
│   └── packages-lock.json
├── ProjectSettings/
├── Documentation/
│   ├── Design/
│   ├── Technical/
│   ├── Narrative/
│   └── QA/
├── Builds/                 # ignoré par Git sauf procédure dédiée
├── .gitignore
└── README.md
```

Règles Git : versionner le code, les scènes, les assets du projet, les fichiers de configuration et les métadonnées `.meta`. Ne pas versionner `Library/`, `Temp/`, `Logs/`, `obj/`, caches locaux ou builds intermédiaires. Utiliser Git LFS pour les gros fichiers binaires qui le justifient. Ne jamais supprimer les `.meta` de manière isolée.

## 17. Démarrage, services et scènes

### 17.1 Scènes

```text
00_Bootstrap
01_MainMenu
02_SeynodRoot
Sectors/VALVERT_NORTH
Sectors/VALVERT_SOUTH
Sectors/VALVERT_EAST
Sectors/VALVERT_WEST
Sectors/SEYNOD_CENTER
Sectors/SCHOOL
Interiors/APARTMENT_KARIM
Interiors/APARTMENT_FATHER
Interiors/SCHOOL_INTERIOR
Tests/TEST_PLAYER
Tests/TEST_SOCIAL_SIM
Tests/TEST_STREAMING
```

`00_Bootstrap` est le point d'entrée du Build Profile. Elle crée le noyau applicatif, vérifie les dépendances, lit les paramètres, puis charge le menu. Le monde est chargé ensuite dans une session indépendante.

### 17.2 Cycle de vie déterministe

Ordre d'initialisation recommandé :

1. Lire les paramètres et la version du build.
2. Initialiser le logger et les diagnostics.
3. Initialiser les services de sauvegarde et de configuration.
4. Initialiser le registre des définitions et identifiants.
5. Charger le menu ou la session demandée.
6. Construire le WorldSession et son contexte.
7. Charger le secteur de départ et ses dépendances.
8. Restaurer l'état de partie ou créer le nouvel état initial.
9. Instancier/activer le personnage contrôlé.
10. Démarrer les systèmes temporels et de simulation.
11. Activer les entrées de jeu.

L'ordre inverse doit être suivi lors de la fermeture : bloquer les commandes, finaliser les sauvegardes, arrêter les systèmes de simulation, décharger les secteurs, puis détruire la session.

### 17.3 Persistance des services

Un `PersistentRoot` peut survivre aux chargements de scène, mais il ne doit pas contenir la totalité du jeu. Il doit être un point de composition. Les services ont une interface claire, une durée de vie, un état d'initialisation et une méthode de fermeture. Les objets de scène sont référencés par leur identifiant de scène ou leur contrat, pas par une hypothèse sur leur présence éternelle.

### 17.4 Gestionnaire de scènes

Le gestionnaire doit fournir :

- chargement/déchargement asynchrones ;
- déduplication des requêtes par ID de secteur ;
- annulation contrôlée si possible ;
- états `Unloaded`, `Loading`, `Loaded`, `Unloading`, `Failed` ;
- journal de progression ;
- restauration de l'état après chargement ;
- protection contre le déchargement du secteur actif ou requis ;
- gestion d'erreur et solution de repli.

Ne pas supposer qu'une opération asynchrone a réussi simplement parce que sa coroutine est terminée : vérifier l'état réel de la scène et ses dépendances.

---

# TOME III — DONNÉES, ÉVÉNEMENTS ET ENTRÉES

## 18. Définitions, instances et données runtime

Les `ScriptableObject` servent aux définitions authorées : profils de déplacement, fiches de personnages, définitions d'objets, dialogues, horaires, lieux, météo et paramètres de caméra. Ils ne doivent pas être utilisés comme conteneurs de progression de partie partagée.

Les états modifiables sont des données runtime sérialisables : position logique, activité, appels manqués, relation, souvenir, état de porte, tâche accomplie, etc. Ils sont convertis en DTO pour être sauvegardés.

### 18.1 Identifiants stables

Chaque entité persistante reçoit un ID stable indépendant du nom du GameObject, du chemin de scène, de l'instance Unity ou de sa position.

Exemples :

```text
CHAR_KARIM
CHAR_TEACHER_001
CHAR_FATHER
PLACE_VALVERT_BENCH_001
PLACE_VALVERT_STREAM_001
DOOR_KARIM_APARTMENT_ENTRY
SECTOR_VALVERT_WEST
DIALOGUE_KARIM_MOTHER_CALL_001
EVENT_KARIM_MISSED_CALL_0001
```

En production, le registre de contenu doit détecter les doublons et empêcher le renommage accidentel d'un ID déjà publié. Un UUID peut compléter un identifiant lisible si des outils d'authoring le nécessitent.

### 18.2 Validation au chargement

Au démarrage, un validateur vérifie : IDs vides ou dupliqués, références manquantes, chemins de scène inexistants, destinations hors secteur, dialogues sans nœud de sortie, horaires contradictoires et définitions incompatibles. Les erreurs bloquantes doivent empêcher la création d'un build Release.

## 19. Commandes et événements

Une **commande** demande une opération : `AnswerPhoneCall`, `OpenDoor`, `StartConversation`, `SitDown`. Un **événement** annonce un fait : `PhoneCallMissed`, `DoorOpened`, `ConversationCompleted`, `SeatOccupied`.

Un système ne doit pas accomplir à la place de tous les autres les conséquences d'un événement. Le téléphone signale qu'un appel a été manqué ; le modèle de relation et la narration évaluent ensuite si quelque chose change.

### 19.1 Enveloppe d'événement

Chaque événement persistant comprend au minimum :

- identifiant d'événement unique ;
- type versionné ;
- temps du monde ;
- acteur et cible éventuels ;
- lieu ou secteur ;
- données utiles typées ;
- origine (action du joueur, routine, scène, système) ;
- visibilité/connaissance potentielle, qui sera ensuite calculée par les systèmes sociaux.

Un événement global n'implique pas que tous les habitants le connaissent.

### 19.2 Événement local et événement durable

Les événements de présentation (poussière, son de pas, animation) ne sont pas nécessairement sauvegardés. Les événements qui influencent une relation, un souvenir, un objet ou une progression doivent être persistés ou résumés de manière reproductible.

L'événementiel doit éviter les doublons : chaque événement ayant des conséquences persistantes possède un ID d'idempotence. Le même événement rejoué après un chargement ne doit pas ajouter deux fois la même conséquence.

## 20. Entrées et modes de contrôle

Créer un fichier `ValVertControls.inputactions` avec au minimum les Action Maps suivantes :

### Player

| Action | Type | Clavier/souris | Manette |
|---|---|---|---|
| Move | Value / Vector2 | ZQSD ou WASD | Stick gauche |
| Look | Value / Vector2 | Souris | Stick droit |
| Run | Button | Maj gauche | Gâchette/bouton retenu |
| Interact | Button | E | Bouton principal |
| Phone | Button | Tab | Bouton View ou bouton choisi |
| ToggleView | Button | V | Bouton affecté |
| Crouch | Button | Ctrl | Bouton affecté |
| Pause | Button | Échap | Menu |

### UI

Navigation, validation, retour, sélection, navigation au clavier et manette. Lorsqu'un écran modal est ouvert, l'UI prend la priorité sur les actions de jeu.

### Developer

F3 mode développeur, déplacement, montée, descente, vitesse, rotation, inspection, téléportation, retour personnage. Cette map n'est jamais activée dans la configuration Release.

### Cinematic

Commandes éventuellement réservées au réalisateur de scènes. Elles ne doivent pas entrer en concurrence avec Player ou UI.

Un seul contrôleur de session est propriétaire de l'activation des actions. Les autres scripts s'abonnent aux événements exposés ou lisent une valeur d'entrée via une interface contrôlée. Aucun module ne doit appeler directement `Enable()` ou `Disable()` sur les mêmes actions de manière indépendante.

---

# TOME IV — PERSONNAGE, CAMÉRAS ET CONTRÔLE DÉVELOPPEUR

## 21. Préfab de base du personnage

Créer `PF_Character_Player_Base` :

```text
PF_Character_Player_Base
├── CharacterController
├── CharacterIdentity
├── PlayerSessionLink
├── PlayerMotor
├── PlayerStateMachine
├── PlayerInteraction
├── CharacterRoot
│   ├── CharacterVisual
│   │   ├── Skeleton
│   │   ├── Meshes
│   │   ├── Animator
│   │   └── RigTargets
│   ├── FirstPersonAnchor
│   │   └── HeadTarget
│   ├── ThirdPersonTarget
│   └── HandTargets
├── InteractionOrigin
└── AudioEmitters
```

La position physique est portée par le CharacterRoot et le contrôleur de mouvement. Les cibles de caméra et les modèles visuels sont des enfants de présentation. La caméra ne déplace jamais le corps physique.

## 22. États du personnage

États locomoteurs : `GroundedIdle`, `Walking`, `Running`, `Crouching`, `Airborne`, `Falling`, `Landing`, `Sitting`, `Interacting`, `UsingPhone`.

Ces états sont séparés des états émotionnels et narratifs. Un personnage peut être contrarié tout en marchant, parler tout en étant assis ou être occupé sans être dans un état d'animation bloquant.

Les états utilisent des transitions vérifiables. Une priorité de contexte empêche, par exemple, qu'une demande d'ouverture du téléphone interrompe une cinématique, une assise en cours ou une conversation verrouillée.

## 23. Profils de déplacement

Valeurs de départ à mesurer dans une scène test, pas des constantes définitives :

| Paramètre | Karim | Professeure | Père |
|---|---:|---:|---:|
| Marche normale | 2,0 m/s | 1,8 m/s | 1,8 m/s |
| Marche rapide/course légère | 4,2 m/s | 3,4 m/s | 3,5 m/s |
| Accélération | 12 m/s² | 9 m/s² | 10 m/s² |
| Décélération | 16 m/s² | 13 m/s² | 14 m/s² |
| Hauteur de capsule | profil modèle | profil modèle | profil modèle |

Les valeurs doivent être ajustées avec les animations, l'échelle réelle des modèles, les escaliers et la sensation au clavier/manette. Le mouvement ne doit pas être trop inertiel ni trop instantané.

### 23.1 Déplacement et collisions

Le moteur de personnage doit gérer gravité, pentes, escaliers, collisions, surface, friction et blocage. Le CharacterController possède une hauteur, un rayon, un Skin Width, un Step Offset et une Slope Limit calibrés pour les portes et les escaliers construits.

Éviter de déplacer le personnage directement par `transform.position +=` comme dans le prototype ECS d'origine : ce déplacement traverse les murs, ignore les NavMeshes et ne gère pas correctement les collisions.

### 23.2 Accroupissement

Modifier de façon coordonnée la hauteur du contrôleur, l'ancre caméra, la vitesse et l'animation. Avant le retour en position debout, faire un test de volume au-dessus de la tête. Si un plafond bloque le mouvement, rester accroupi.

### 23.3 Assise

Chaque siège possède un ou plusieurs `SeatPoint` : pose, orientation, zone d'occupation, animation compatible et ID stable. L'action vérifie la disponibilité, rejoint le point de manière contrôlée, réserve le siège, joue l'animation et autorise les actions compatibles (regarder, parler, téléphone). Le siège est libéré au lever ou lors d'une interruption valide.

## 24. Caméras de jeu

### 24.1 Trois familles distinctes

1. **Première personne** : vue à hauteur des yeux de Karim ou du personnage actif.
2. **Troisième personne** : corps visible, caméra suiveuse orbitale et collision.
3. **Cinématique** : caméra pilotée par une scène, une timeline ou un rig spécifique.

La caméra libre développeur appartient à un autre système et n'est pas une quatrième vue gameplay.

### 24.2 Configuration Cinemachine

Créer les rigs `CM_FirstPerson`, `CM_ThirdPerson` et `CM_Cutscene`. Utiliser une seule caméra Unity de rendu, avec un `CinemachineBrain`. La caméra active est choisie par priorité ou par un contrôleur central. Ne pas activer/désactiver plusieurs caméras Unity de rendu simultanément sur le même écran.

La troisième personne doit gérer les collisions avec les murs, les angles, les passages étroits, les escaliers et les plafonds. Lorsqu'il n'y a pas de place, la caméra se rapproche du personnage ou passe à une composition acceptable plutôt que de traverser la géométrie.

### 24.3 Visibilité du corps

En première personne, masquer seulement les éléments susceptibles de gêner l'image (tête/torse selon le rig) tout en conservant les ombres et les éléments nécessaires aux interactions. Les mains sont affichées lorsqu'une action le demande. En troisième personne, le corps complet est visible. Les autres habitants voient toujours la représentation physique du personnage.

### 24.4 Changement de vue

Appuyer sur V change le mode de caméra, pas le personnage. Le changement ne touche ni position, ni état narratif, ni relations, ni inventaire, ni sauvegarde. Le système doit vérifier que le contexte autorise le changement et effectuer une transition sans saut de caméra.

Tests : position et rotation corporelle inchangées, interactions encore valides, pas de double input, pas de changement en pause/écran de téléphone/cinématique verrouillée, pas de traversement de mur en troisième personne.

## 25. Mode développeur et caméra libre

### 25.1 Autorisation

Le mode développeur est réservé à l'éditeur et aux builds internes explicitement autorisés. Il ne fait pas partie du récit et ne constitue pas une capacité de Karim.

La protection combine :

- `#if UNITY_EDITOR || DEVELOPMENT_BUILD` autour des entrées et composants concernés ;
- exclusion des objets et commandes développeur des builds Release ;
- absence d'activation de la map Developer en Release ;
- validation du Build Profile avant compilation ;
- test automatisé vérifiant l'absence de l'API développeur en Release.

Masquer un bouton UI n'est pas une protection suffisante.

### 25.2 Contrôles développeur

| Touche | Fonction |
|---|---|
| F3 | Activer/désactiver la caméra libre |
| ZQSD | Déplacement horizontal |
| Espace | Monter |
| Ctrl | Descendre |
| Maj | Accélérer |
| Alt | Ralentir |
| Souris | Orienter la caméra tant que le bouton de rotation est maintenu |
| F4 | Overlay développeur |
| F5 | Reprendre le personnage |
| F6 | Copier les coordonnées |
| F7 | Afficher les volumes de streaming |

### 25.3 Architecture

```text
Developer/
├── Runtime/
│   ├── DeveloperModeController
│   ├── DeveloperInputRouter
│   ├── FreeFlyCameraController
│   ├── DeveloperOverlay
│   ├── DeveloperTeleportService
│   ├── DeveloperWorldInspector
│   └── DeveloperCursorState
├── Editor/
│   ├── ValVertWorldWindow
│   ├── SectorGizmos
│   ├── SpawnPointEditor
│   ├── SchedulePreviewWindow
│   └── ValidationMenu
└── Settings/
    └── DeveloperSettings
```

### 25.4 Modes de vitesse

- Précision : environ 1 m/s.
- Normal : environ 3 m/s.
- Quartier : environ 10 m/s.
- Ville : environ 35 m/s.
- Transfert très rapide : environ 100 m/s.

Toutes les vitesses sont configurables. La caméra utilise un delta non dépendant du temps de simulation du monde, de sorte que le développeur puisse se déplacer même lorsque la simulation narrative est en pause.

### 25.5 Transfert du contrôle

Lors de l'activation : mémoriser l'état précédent du curseur et de la map d'entrée, désactiver les commandes joueur, geler ou laisser tourner séparément la simulation selon l'option de debug, rendre la caméra libre active, puis capturer le curseur. Lors du retour : restaurer l'état exact de pause, la map d'entrée et le curseur, réactiver la caméra de jeu et le contrôleur du personnage.

Ne pas simplement désactiver un objet `playerCamera` : le pipeline Cinemachine, le Brain, l'état de curseur et les actions Input System doivent rester cohérents.

### 25.6 Fonctions d'inspection

Overlay : coordonnées, rotation, ID de secteur, scène active, FPS, temps du monde, nombre d'habitants proches/intermédiaires/lointains, état de streaming, nombre d'objets chargés, simulation active/pausée.

Outils : téléportation vers un point nommé, personnage, porte, bâtiment, zone narrative ou secteur ; copie des coordonnées ; inspection des colliders, capsules, zones d'interaction, NavMesh, sièges et volumes de streaming ; sélection d'un habitant pour afficher son activité, sa destination, ses relations de diagnostic et ses connaissances, sans modifier la partie par défaut.

---

# TOME V — PERSONNAGES, INTERACTIONS ET NARRATION

## 26. Changement de personnage jouable

Le changement de personnage normal est une décision narrative. Le jeu vérifie qu'un changement est autorisé, enregistre l'état courant, arrête les commandes, charge le lieu et les dépendances du personnage cible, restaure son état, active ses contrôles et caméra, puis reprend le temps.

Chaque protagoniste possède un `PlayableCharacterDefinition` : ID, nom d'affichage, prefab, profil de mouvement, contrôleur d'animation, profil caméra, point de départ, profil narratif et clé de sauvegarde.

Le mode développeur peut forcer le changement à des fins de test, mais le panneau de debug ne doit jamais être exposé au joueur normal.

Les personnages non actifs continuent d'exister selon leur niveau de simulation. Le changement ne doit pas les téléporter arbitrairement vers le protagoniste actif.

## 27. Système d'interaction universel

Contrat conceptuel :

```csharp
public interface IInteractable
{
    bool CanInteract(InteractionContext context);
    string GetInteractionLabel(InteractionContext context);
    InteractionResult Interact(InteractionContext context);
}
```

`InteractionContext` comprend l'acteur, l'objet visé, la distance, le lieu, le temps du monde, l'état de scène et les autorisations narratives. `InteractionResult` indique si l'action a commencé, échoué, est différée ou a déclenché une scène.

Le contrôleur d'interaction :

1. Lance un raycast depuis l'origine d'interaction.
2. Vérifie le layer et la portée.
3. Résout l'objet logique et son ID stable.
4. Interroge `CanInteract`.
5. Affiche un prompt fourni par l'objet/contextualisé.
6. Émet une commande sur action du joueur.
7. Reçoit le résultat et publie les événements pertinents.

Les interactions ne sont pas codées en dur dans le contrôleur de joueur.

## 28. Objets du quotidien et activités

Définitions de base : `ItemDefinition`, `InteractableDefinition`, `SeatDefinition`, `DoorDefinition`, `ActivityDefinition`. Les objets runtime conservent leur état sous forme de données (porte ouverte, objet pris, tasse déplacée) si ce changement a une valeur narrative ou de continuité.

Activités prévues : s'asseoir, attendre, lire, manger, boire, consulter un document, utiliser un téléphone, regarder une télévision, préparer ou ranger un objet, rentrer chez soi, étudier ou travailler selon le personnage.

Une activité possède des conditions d'entrée, une durée éventuelle, un point d'interaction, une animation ou séquence, une règle d'interruption, un résultat et les événements qu'elle peut produire.

## 29. Téléphone et communication

### 29.1 Structure

```text
PhoneRuntimeState
├── OwnerCharacterId
├── Contacts
├── Conversations
├── Messages
├── Calls
├── MissedCalls
├── CalendarEntries
├── NotificationQueue
└── LastOpenedApplication
```

Les messages et appels sont des données narratives, pas uniquement des widgets. Chaque message a un expéditeur, un destinataire, un temps d'envoi, un contenu localisé, un état lu/non lu, une conversation et des règles de visibilité.

### 29.2 Appel entrant de la mère

Événement d'exemple : `KARIM_MOTHER_CALL_001`, heure initiale 19:00. Choix : répondre, laisser sonner, refuser, puis éventuellement lire le message ou rappeler.

Conséquences possibles : appel répondu/manqué, inquiétude, attente au repas, message ultérieur, conversation différée. Rien ne doit être automatique : le père ne sait pas forcément qu'un appel a été ignoré ; un ami présent peut ne pas connaître le contenu de l'appel ; la mère peut interpréter le silence de plusieurs façons.

### 29.3 Téléphone en main

Le téléphone a un état logique (fermé, sorti, affiché, rangé) et une animation de personnage. L'interface ne doit pas s'ouvrir sans la transition corporelle prévue, sauf règle d'accessibilité ou option de confort. Les appels importants peuvent interrompre l'interface selon la priorité de scène.

## 30. Dialogues et scènes narratives

Un dialogue de données contient : ID, intervenants, lieu ou portée, conditions, nœuds de réplique, choix, conséquences commandées, conditions de sortie, voix/sous-titres, tags et état de complétion.

Les dialogues sont data-driven. Les conditions consultent des faits connus (pas la vérité globale non partagée), les relations, les promesses, le temps, l'activité et les personnes présentes.

### 30.1 Modèle de nœud

Un nœud peut contenir un locuteur, un texte localisé, des conditions d'affichage, une animation/attitude, une durée, une liste de réponses et les transitions correspondantes. Les conditions doivent être testables et affichables dans un outil de debug.

### 30.2 Éviter les conséquences artificielles

Les réactions ne se produisent que si le personnage a vu, entendu, appris ou déduit l'information. Une conversation ne doit pas accorder automatiquement une connaissance à toute la population.

### 30.3 Scènes temporelles

Les scènes peuvent être interrompues, ratées ou modifiées par l'heure, la présence, l'activité ou un événement antérieur. Le système doit définir explicitement quelles scènes sont obligatoires, facultatives, rejouables ou définitivement manquées.

---

# TOME VI — RELATIONS, MÉMOIRE ET IA SOCIALE

## 31. Relation entre deux personnages

Une relation est multidimensionnelle et orientée par paire : confiance, familiarité, affection, respect, tension, méfiance, inquiétude et historique des interactions. Les valeurs restent internes. Le jeu n'affiche pas une jauge de relation.

La relation ne doit jamais être égale à une somme opaque de points. Les comportements consultent des dimensions pertinentes et des faits précis. Deux personnes peuvent être proches tout en étant en désaccord, ou se respecter sans être intimes.

## 32. Mémoire événementielle

Une mémoire doit distinguer :

- le fait observable (ce qui s'est réellement produit) ;
- la perception (ce que le personnage a vu/entendu) ;
- l'interprétation (ce qu'il croit que cela signifie) ;
- l'information rapportée (ce qu'une autre personne lui a raconté) ;
- le souvenir résumé (ce qui reste retenu dans le temps).

Données possibles : MemoryId, EventId, temps, lieu, personnes impliquées, description factuelle, source de connaissance, interprétation, importance, émotion associée, confiance, dernière réactivation et statut de rétention.

Exemple : la mère sait que Karim n'a pas répondu à l'appel si elle l'a appelé et a constaté qu'il n'a pas décroché. Elle ne sait pas nécessairement pourquoi. Elle peut penser qu'il est occupé, qu'il n'a pas entendu le téléphone ou qu'il évite de rentrer. Une interprétation ne doit pas être stockée comme un fait objectif.

## 33. Circulation de l'information

Sources autorisées : observation visuelle, conversation entendue, conversation directe, appel, message, document, rumeur ou déduction.

Chaque transfert d'information contient un émetteur, un receveur, une source initiale, un temps, un contenu/événement, un niveau de confiance et éventuellement une déformation. La personne qui reçoit n'obtient pas nécessairement la vérité complète.

## 34. Oubli et consolidation

La mémoire ne doit pas grandir sans limite. Les détails ordinaires sont regroupés en résumés ; les événements majeurs restent persistants ; les souvenirs régulièrement réactivés peuvent devenir plus accessibles ; les détails peu importants peuvent perdre en précision sans supprimer les conséquences importantes.

Ne pas faire disparaître un fait critique de sauvegarde simplement parce qu'une personne l'a oublié. Le modèle narratif et le système de mémoire personnelle sont deux couches différentes.

## 35. Simulation de population à trois niveaux

| Niveau | Contexte | Simulation |
|---|---|---|
| Proche | Visible, audible ou interactif | GameObject, NavMesh, Animator, regard, dialogue, perception et réactions détaillées. |
| Intermédiaire | Même quartier ou secteur chargé | Activité et destination logiques, déplacement simplifié sur réseau de chemins, interactions importantes. |
| Lointain | Secteur non chargé | État abstrait, horaires, activité prévue, événements et progression temporelle. |

Le niveau de simulation dépend de la distance, de la visibilité, de l'importance narrative et du besoin de continuité. Les protagonistes et habitants narrativement importants peuvent rester suivis en logique même loin du joueur.

## 36. Horaires et routines

Un planning est un ensemble d'activités avec heure/intervalle, lieu cible, priorité, durée, flexibilité, conditions et stratégie de repli. Le système ne doit pas être un simple script qui téléporte l'habitant à 08:00.

En cas de conflit : événement narratif prioritaire, sécurité/accès, activité courante, routine normale, puis activité de repli. Si un trajet est impossible, l'habitant attend, choisit une alternative ou actualise son état logique ; il ne traverse pas les murs.

Les horaires sont des intentions et des contraintes, pas une garantie d'exécution exacte à la seconde.

## 37. Activités autonomes

Exemples : marcher vers un lieu, attendre, regarder un point, s'asseoir, lire, discuter, rentrer, sortir, utiliser un téléphone, manger ou rejoindre un groupe. Chaque activité a un contexte, une destination, un état, une durée, une condition de sortie et une conséquence potentielle.

Le système de comportement choisit une activité compatible avec le planning, le lieu, les relations, l'état et les événements connus. Il faut éviter les réactions aléatoires sans cause lisible.

## 38. ECS : périmètre contrôlé

ECS est réservé aux traitements de masse mesurés : état logique de populations nombreuses, recherches de proximité, calcul d'horaires simplifiés, agrégation et déplacements grossiers sur réseau. Les dialogues complexes, animations détaillées, interactions et scènes restent côté GameObject/services C# classiques.

Ne pas adopter ECS uniquement parce que le projet est ambitieux. Construire d'abord un prototype mesuré avec GameObjects, puis déplacer seulement le goulot d'étranglement vers ECS.

Composants possibles : `ResidentTag`, `ResidentIdentity`, `ResidentPosition`, `ResidentScheduleState`, `ResidentActivityState`, `ResidentSimulationTier`, `ResidentRouteState`. Les composants restent des données simples. Les systèmes ECS effectuent des calculs déterministes et sont isolés de la présentation.

La synchronisation entre données ECS et représentants GameObject est explicite, en particulier lors des changements de niveau de simulation et du streaming.

---

# TOME VII — NAVIGATION, SEYNOD ET MONDE OUVERT

## 39. Navigation des habitants

Les habitants proches utilisent des NavMesh adaptés aux zones réellement accessibles. Les routes piétonnes, escaliers, portes, entrées d'immeubles, chemins du bois, traversées et passages doivent être représentés par une topologie contrôlée.

Le réseau de déplacement comprend :

- surfaces de marche ;
- liens spéciaux (escaliers, rampes, portes, passages) ;
- zones interdites ou temporaires ;
- points d'intérêt ;
- traversées nécessitant une animation ou une règle de sécurité ;
- alternatives de route.

À moyenne distance, la simulation peut utiliser un graphe de lieux et de segments, sans pathfinding au centimètre. À grande distance, la progression est abstraite.

## 40. Recherche de proximité et interactions de foule

Ne pas comparer naïvement chaque habitant à tous les autres à chaque frame. Utiliser une grille spatiale, des cellules, des arbres ou les structures appropriées au niveau de simulation. Le choix se fait selon les mesures et le volume de population.

Les interactions de groupe doivent tenir compte du contexte : qui peut entendre, qui est disponible, qui connaît déjà le sujet, combien de personnes peuvent participer, comment un personnage se retire et comment la conversation se termine.

## 41. Reconstruction géographique de Seynod

### 41.1 Références

Utiliser des fonds cartographiques, plans, relevés, photos et données géographiques dont les droits d'utilisation sont vérifiés. Les références servent à mesurer et reconstruire ; elles ne doivent pas être redistribuées dans le jeu si la licence ne l'autorise pas.

### 41.2 Pipeline de reconstruction

1. Définir le périmètre géographique et les lieux narratifs prioritaires.
2. Fixer un système de coordonnées local métrique.
3. Importer les références dans un outil adapté.
4. Tracer routes, chemins, emprises, limites de bâtiments et altitudes utiles.
5. Nettoyer les géométries et contrôler les distances.
6. Construire terrain, routes, chemins et volumes de bâtiment.
7. Ajouter les proxies de collision.
8. Ajouter les modèles détaillés et matériaux.
9. Installer NavMesh, points d'accès et repères d'interaction.
10. Vérifier les dimensions en jeu avec le personnage.

### 41.3 Système de coordonnées

Ne pas utiliser des coordonnées géographiques brutes comme positions Unity. Définir une origine locale proche de la zone jouable, une projection métrique adaptée, l'échelle, l'orientation, les unités, et une procédure de conversion documentée. Conserver les coordonnées source et les coordonnées Unity dans les données d'import lorsque cela est utile. Éviter les très grandes coordonnées flottantes sans stratégie d'origine/precision.

### 41.4 Fidélité et limites

La géométrie utile à la navigation et à la narration est prioritaire sur la reproduction photogrammétrique exhaustive. Les bâtiments non visitables peuvent être des proxies extérieurs. Les lieux majeurs nécessitent des intérieurs et détails spécifiques.

## 42. Secteurs et découpage du monde

Chaque secteur est une unité de chargement, d'authoring, de diagnostic et de restauration. Il possède un ID stable, des bounds, des références de scènes, les secteurs voisins, ses points d'intérêt, ses dépendances et son niveau de priorité.

Proposition initiale : `VALVERT_NORTH`, `VALVERT_SOUTH`, `VALVERT_EAST`, `VALVERT_WEST`, `RESIDENTIAL_01`, `RESIDENTIAL_02`, `SEYNOD_CENTER`, `SCHOOL`. Le découpage doit être corrigé selon la topologie réelle du monde, pas décidé uniquement par une grille arbitraire.

## 43. Streaming hybride

Le gestionnaire coordonne :

- scènes additives Unity pour les secteurs GameObject et intérieurs ;
- SubScenes pour les données ECS bakées si leur usage est retenu ;
- données persistantes indépendantes du contenu chargé ;
- restauration après chargement ;
- activation/désactivation des systèmes de navigation, audio et simulation.

Un secteur n'est pas « chargé » simplement parce que son fichier de scène est présent. Il doit passer par les étapes : fichier/scène chargée, dépendances prêtes, objets persistants restaurés, navigation disponible, système audio/configuration actif, puis gameplay activé.

### 43.1 Anticipation et déchargement

Charger les secteurs proches avant qu'ils ne deviennent nécessaires. Le rayon de préchargement doit être supérieur au rayon de déchargement afin d'éviter les oscillations. Un secteur contenant une scène, une interaction ou un trajet actif ne doit pas être déchargé prématurément.

### 43.2 État des objets

Une porte ouverte, un objet déplacé ou une activité terminée est stocké par ID stable dans l'état du monde. Le prefab chargé n'est que la représentation. Lors du retour dans le secteur, l'état est réappliqué avant d'activer les interactions.

## 44. Intérieurs, portes et mobilier

Les portes ont un état logique, une animation, un sens d'ouverture, une règle de verrouillage, un point d'interaction, une zone de passage et éventuellement une transition d'intérieur. Les collisions doivent s'adapter à l'état ouvert/fermé.

Les intérieurs peuvent être chargés en additif, en scène indépendante ou sous forme de cellule/secteur. Le passage extérieur/intérieur doit conserver une position de retour, le temps du monde, l'ambiance sonore, la lumière et l'état des portes.

Les meubles interactifs (chaises, lits, tables, télévisions) utilisent des points d'usage et des réservations. Ne pas permettre à deux personnages de s'asseoir dans le même siège ou d'occuper le même emplacement d'animation.

---

# TOME VIII — TEMPS, MÉTÉO ET AMBIANCE

## 45. Horloge du monde

Distinguer trois horloges :

- temps réel non mis à l'échelle pour interface, input, animations UI et outils développeur ;
- temps de simulation pour les agents et systèmes ;
- temps du monde pour date, heure, horaires et narration.

Ne pas détourner `Time.timeScale` pour faire avancer l'horloge sociale. Une pause peut suspendre la simulation sans changer l'heure du monde, tandis qu'une transition de menu ou de sauvegarde peut conserver des services non liés au monde.

L'horloge fournit date, heure, jour, durée écoulée, multiplicateur configurable et événements de franchissement de seuil. Les systèmes de routine s'abonnent à des changements significatifs plutôt que d'interroger l'heure à chaque frame.

## 46. Simulation hors écran et rattrapage temporel

Lors du retour dans un secteur, le système calcule l'intervalle écoulé et met à jour les habitants lointains par étapes bornées. Ne pas simuler chaque frame manquante. Utiliser une progression abstraite et des transitions d'activité. Limiter le rattrapage par batch pour ne pas créer de pic CPU.

Le résultat doit être cohérent et reproductible à partir de l'heure, des états et événements sauvegardés. Les événements importants doivent avoir un ordre déterministe.

## 47. Météo, lumière et saisons

États météo initiaux : clair, nuageux, couvert, pluie légère, pluie forte, brouillard et éventuellement neige si le scénario et la localisation le justifient. Chaque état possède intensité, transition, effets visuels, audio et conséquences de comportement.

La météo influence les matériaux, particules, sons, lumière, visibilité et choix d'activité. Exemple : un banc mouillé est moins attractif ; un habitant peut choisir un chemin couvert ; une averse peut modifier le rythme de marche.

Le cycle jour/nuit ajuste le soleil, les ombres, le ciel, les éclairages publics, les intérieurs et les ambiances. Les valeurs doivent être mesurées et artistiquement validées à plusieurs heures.

---

# TOME IX — OBJETS, AUDIO, VISUELS ET INTERFACE

## 48. Inventaire du quotidien

L'inventaire est limité aux objets utiles à la vie et à la narration : téléphone, clés, documents, sac, objets personnels, nourriture, fournitures scolaires et éléments d'histoire. Pas de système d'équipement offensif.

Il distingue définition (ce qu'est l'objet), instance (l'objet particulier), quantité, propriétaire, état, emplacement et historique si nécessaire. Les objets du monde et les objets portés ne sont pas forcément le même type de stockage.

Les opérations `Add`, `Remove`, `Transfer`, `Use`, `Drop` et `Inspect` sont des commandes validées par les règles de jeu. Les transferts et prises doivent éviter les doublons lors d'une sauvegarde/reprise.

## 49. Audio

Groupes de mixage : Master, Ambience, Environment, Characters, Dialogue, Phone, UI et Music si une musique est utilisée. Les sons extérieurs et intérieurs doivent être spatialisés et atténués selon les murs, la distance et l'environnement.

Couches : ambiance générale de quartier, bois et ruisseau, circulation, oiseaux, pas, vêtements, conversations, téléphone, pièces intérieures et événements ponctuels.

Le pooling peut être utilisé pour les sources temporaires fréquentes. Les dialogues doivent avoir une priorité et un système de ducking. Sous-titres synchronisés, contrôle des volumes par groupe et mode sans musique sont prévus.

## 50. Graphismes URP

Priorités : lisibilité de l'espace, direction artistique cohérente, stabilité d'éclairage, lisibilité des matériaux, qualité des silhouettes et performances.

À mettre en place progressivement : LOD, occlusion culling, instancing, matériaux partagés, textures adaptées, terrain par secteurs, végétation optimisée, ombres calibrées, Light Probes, Reflection Probes et gestion des objets lointains. Les habitants éloignés n'ont pas besoin d'animations complètes.

Le niveau de qualité doit être validé sur la machine cible. Ne pas décider du nombre de lumières temps réel, de la distance d'ombre ou de densité de végétation sans profiler une scène représentative.

## 51. Animation et regard

L'Animator expose des paramètres de locomotion et de posture (Speed, MoveX, MoveY, IsRunning, IsCrouching, IsGrounded, IsSitting, IsUsingPhone, IsInteracting). Les émotions sont des paramètres d'expression ou des poses, non une note numérique affichée.

Un système de regard peut sélectionner une personne, un objet, un téléphone ou une position de scène. Le regard doit être subtil, temporisé et interrompable. Les animations ne doivent pas forcer un changement de position physique non prévu par le moteur de déplacement.

## 52. Interface utilisateur

Écrans prévus : menu principal, sélection/chargement de partie, pause, téléphone, dialogue, interaction contextuelle, réglages, sous-titres, sauvegarde et interface développeur uniquement en build interne.

L'interface de jeu reste discrète. Pas de jauge de vie, score, expérience ou relation. Les informations apparaissent lorsqu'elles sont utiles. Certaines scènes peuvent masquer l'interface entièrement.

### 52.1 Accessibilité

- Remappage des touches principales.
- Options clavier/souris/manette.
- Sensibilité et inversion des axes.
- Champ de vision réglable.
- Désactivation ou réduction du head bob, du flou de mouvement et des secousses.
- Sous-titres configurables (taille, contraste, nom du locuteur).
- Contraste et taille de texte.
- Options de maintien/appui pour les actions continues.
- Pause possible dans les menus hors scènes verrouillées, selon les règles du récit.

### 52.2 Localisation

Tous les textes visibles doivent passer par des clés localisées, pas des chaînes codées en dur dans les scripts. Les noms propres et expressions locales peuvent avoir des règles spécifiques. Prévoir la gestion des accents, pluriels, longueurs de texte, retours de ligne et sous-titres.

---

# TOME X — SAUVEGARDE ET PERSISTANCE

## 53. Architecture de sauvegarde

La sauvegarde est indépendante des prefabs et scènes chargés. Elle contient des DTO versionnés :

```text
SaveGame
├── SchemaVersion
├── GameBuildVersion
├── SaveTimestampUtc
├── WorldClock
├── ActiveCharacterId
├── CharacterStates[]
├── RelationshipStates[]
├── MemoryStates[]
├── KnowledgeStates[]
├── NarrativeStates[]
├── PhoneStates[]
├── InventoryStates[]
├── WorldObjectStates[]
├── ResidentScheduleStates[]
├── SectorStates[]
└── UserSettingsReference
```

Ne jamais sérialiser directement les références Unity (`GameObject`, `Transform`, `MonoBehaviour`, `ScriptableObject` d'asset) dans la sauvegarde. Stocker leurs identifiants stables et résoudre les références au chargement.

## 54. Écriture fiable

Procédure :

1. Capturer un instantané cohérent de l'état de session.
2. Construire un DTO indépendant des objets Unity.
3. Sérialiser vers un fichier temporaire.
4. Fermer et vérifier l'écriture.
5. Remplacer atomiquement la sauvegarde principale lorsque la plateforme le permet.
6. Conserver une sauvegarde précédente/backup.
7. Écrire un checksum ou une vérification d'intégrité.
8. Mettre à jour le manifeste de slot.

Les sauvegardes automatiques se déclenchent à des points sûrs : fin de scène majeure, changement de secteur critique, changement de protagoniste, sortie propre et intervalles raisonnables. Éviter une écriture complète à chaque interaction.

## 55. Reprise et migration

Au chargement : vérifier le fichier, sa version, son intégrité et la présence de ses données essentielles. Si le fichier est corrompu, tenter le backup. Si la version est ancienne, exécuter une migration versionnée. Si elle est inconnue, ne pas écraser silencieusement la sauvegarde ; afficher une erreur claire et préserver le fichier original.

Les migrations sont testées avec des fixtures de versions antérieures. Une nouvelle version du jeu ne doit pas invalider une sauvegarde simplement parce qu'un nom de classe C# a changé.

## 56. Journal d'événements et instantanés

Pour les données critiques, une architecture hybride peut conserver un instantané et un journal d'événements persistants depuis cet instantané. Elle doit rester limitée : journaliser tous les mouvements d'animation serait inutile. Le journal est adapté aux événements significatifs et idempotents.

---

# TOME XI — OUTILS DE PRODUCTION DANS UNITY

## 57. Fenêtre Val Vert Tools

Créer une fenêtre Editor avec les panneaux suivants :

**WORLD** — Create Sector, Validate Sector, Show Bounds, Add Landmark, Export Coordinates.  
**CHARACTERS** — Create Resident, Add Spawn Point, Assign Routine, Preview Schedule.  
**NARRATIVE** — Create Trigger, Dialogue Point, Event Definition, Memory Rule.  
**ENVIRONMENT** — Create Door, Seat, Interaction, Interior Link.  
**NAVIGATION** — Bake/Validate NavMesh, Inspect Off-Mesh Links, Show Route Graph.  
**TESTING** — Validate Scene, Find Missing IDs, Check Collisions, Check Streaming, Check Save Data.

## 58. Outils d'authoring

- Inspecteurs personnalisés pour définitions, profils et routines.
- Outils de placement de sièges avec orientation et zone d'occupation.
- Outils de placement de points d'apparition.
- Visualisation des secteurs, bounds et voisinages.
- Éditeur visuel de routines et calendrier.
- Prévisualisation de la journée d'un résident.
- Inspecteur de dialogue avec aperçu des conditions.
- Création de volumes d'interaction.
- Importateur de données géographiques et contrôle des coordonnées.
- Génération de rapports de validation.

## 59. Validation automatisée du contenu

Un outil doit détecter au minimum :

- ID manquant ou doublon ;
- référence de prefab absente ;
- scène absente des Build Profiles ;
- secteur sans bounds ou sans ID ;
- porte sans destination ou sans collision cohérente ;
- siège sans point d'assise ;
- résident sans point d'attache ou routine de repli ;
- itinéraire non navigable ;
- dialogue inaccessible ou choix sans transition ;
- clé de localisation manquante ;
- référence d'asset invalide ;
- données persistantes non sérialisables.

Les validations bloquantes échouent en CI et interdisent le build Release.

---

# TOME XII — TESTS, PERFORMANCE ET LIVRAISON

## 60. Stratégie de test

### 60.1 EditMode

Tester les contrats de données, identifiants, validations, règles de relation, sélection de dialogue, calculs d'horaires, migration et sérialisation sans lancer une scène complète.

### 60.2 PlayMode

Tester déplacement, caméras, interactions, téléphone, entrée/sortie d'intérieur, navigation, chargement/déchargement secteur, activation de population, transition de personnage et sauvegarde/récupération.

### 60.3 Tests de parcours

Exemples de parcours automatisés :

- nouvelle partie Karim → appel entrant → ignorer → sauvegarder → recharger → retrouver l'appel manqué ;
- entrer dans un bâtiment → ouvrir une porte → quitter secteur → recharger → porte encore ouverte ;
- changer de caméra → poursuivre déplacement → interaction cohérente ;
- changer de protagoniste → revenir plus tard → retrouver activité et lieu logiques ;
- mettre en pause → activer le mode développeur → revenir au joueur → état de pause identique ;
- laisser passer plusieurs heures hors écran → revenir → habitants à des états cohérents ;
- build Release → aucune commande de vol libre ou d'inspection développeur accessible.

### 60.4 Non-régression

Toute nouvelle fonction est testée avec les systèmes existants. Une modification caméra ne doit pas casser interaction, téléphone, animation, sauvegarde ou changement de protagoniste. Une modification de mémoire ne doit pas faire connaître un événement à un témoin absent.

## 61. Performance et budgets

Les budgets sont des objectifs initiaux à mesurer et à ajuster selon le matériel cible, pas des résultats déjà obtenus.

- Définir un framerate cible par plateforme et un profil minimal.
- Mesurer CPU, GPU, mémoire, chargement, GC, animation, navigation, rendu et streaming.
- Éviter les allocations répétées dans `Update`.
- Utiliser pooling uniquement pour les objets à création/destruction fréquente (sons, notifications, effets de présentation, éléments de foule temporaires).
- Définir des limites de résidents proches, animations actives, sources audio et objets visibles à partir des mesures.
- Utiliser Burst/ECS si le profilage démontre un gain.
- Effectuer les mesures dans un build représentatif, pas seulement dans l'éditeur.

## 62. Profiling

Outils : Unity Profiler, Profile Analyzer, Memory Profiler, Frame Debugger, Burst Inspector et captures de chargement. Chaque optimisation doit indiquer le problème mesuré, le changement, le résultat et les éventuels compromis visuels ou fonctionnels.

## 63. Build Profiles et sécurité

Prévoir :

| Profil | Usage | Outils développeur |
|---|---|---|
| Editor | Création et débogage | Oui |
| Development | Tests internes instrumentés | Oui, protégés |
| QA | Validation reproductible | Selon politique QA |
| Release | Version publique | Non |

Les outils de vol, téléportation, inspection interne et overlays de diagnostic doivent être exclus ou neutralisés en Release par compilation et vérification du profil. Les logs de debug ne doivent pas exposer de données privées ou internes dans la version publique.

## 64. Intégration continue et livraison

Pipeline recommandé :

1. Vérification Git et fichiers attendus.
2. Résolution des packages verrouillés.
3. Compilation sans erreur.
4. Tests EditMode.
5. Tests PlayMode ciblés.
6. Validation des assets, IDs, scènes et dialogues.
7. Build Development/QA.
8. Smoke test du build.
9. Archivage du build, logs, version moteur et résultat des tests.

Chaque version doit être identifiée par numéro de build, commit Git, version Unity, liste des packages, profil de build et changelog. Une livraison n'est validée qu'après test sur une machine cible représentative.

---

# TOME XIII — ORDRE DE RÉALISATION DE A À Z

## 65. Phase 0 — Cadrage et préproduction

**Livrables** : vision validée, périmètre, personnages, carte de lieux, liste des interactions, scénario pilote, budget matériel cible, registre des risques et règles de données.

**Validation** : le scénario pilote peut être décrit de bout en bout sans dépendre de fonctions non définies.

## 66. Phase 1 — Projet Unity propre

1. Installer Unity 6.6 LTS et l'IDE.
2. Créer le projet Universal 3D `ValVertII`.
3. Résoudre les packages et verrouiller le manifeste.
4. Créer le dépôt Git et vérifier `.gitignore`/LFS.
5. Créer les assemblies initiales.
6. Créer `00_Bootstrap`, `01_MainMenu`, `02_SeynodRoot`.
7. Construire et lancer un build vide.

**Validation** : ouverture/fermeture du projet, résolution des packages, compilation et build propres.

## 67. Phase 2 — Noyau applicatif

1. PersistentRoot / composition root.
2. SceneLoader asynchrone.
3. WorldSession.
4. Logger et diagnostics.
5. WorldEventBus typé.
6. Input System et gestionnaire de modes.
7. registre d'identifiants et validateur.

**Validation** : aucun service dupliqué, démarrage déterministe, événements testés, entrées activées/désactivées sans conflit.

## 68. Phase 3 — Prototype vertical du joueur

1. Construire une scène de test à échelle réelle.
2. Créer le prefab de Karim.
3. Déplacement, gravité, escaliers, collision.
4. Caméra subjective.
5. Caméra troisième personne.
6. Transition de vue.
7. Animation de marche/course/arrêt.
8. Interaction simple et assise.

**Validation** : 10 minutes de test continu, pas de blocage, caméra sans collision critique, interactions reproductibles.

## 69. Phase 4 — Prototype narratif du téléphone

1. Créer le téléphone et son UI.
2. Appel entrant de la mère.
3. Répondre / ignorer / refuser.
4. Émettre les événements.
5. Mettre à jour la mémoire de la mère.
6. Ajouter une réaction différée.
7. Sauvegarder et recharger.

**Validation** : le résultat persiste, les personnages ne connaissent que ce qu'ils peuvent savoir, le dialogue ultérieur reflète la situation.

## 70. Phase 5 — Boucle sociale

1. Deux résidents avec IDs persistants.
2. Un horaire simple.
3. Une conversation entre eux.
4. Perception et circulation d'information.
5. Relation multidimensionnelle.
6. Mémoire, résumé et oubli.
7. Niveau proche/intermédiaire/lointain.

**Validation** : l'habitant existe sans modèle actif, revient à un état cohérent, et ses connaissances ne sont pas omniscientes.

## 71. Phase 6 — Premier secteur réel

1. Référencer et mesurer une petite zone du bois du Val Vert.
2. Fixer origine et coordonnées.
3. Construire terrain, chemin, ruisseau/bassin si dans le périmètre pilote.
4. Ajouter bancs, points d'assise, collisions, éclairage et audio.
5. Ajouter points d'intérêt et habitants.
6. Ajouter un secteur adjacent et le streaming.

**Validation** : parcours fluide, limites de secteur correctes, navigation cohérente, états persistants après déchargement.

## 72. Phase 7 — Intérieurs et parcours des trois protagonistes

1. Construire un intérieur narratif de Karim.
2. Construire les liens de porte et sauvegarder leurs états.
3. Ajouter un lieu de travail/école nécessaire à la professeure.
4. Ajouter un lieu nécessaire au parcours du père.
5. Implémenter changement de personnage narratif.
6. Tester les conséquences croisées.

**Validation** : les trois protagonistes ont des séquences jouables et le monde partagé reste cohérent.

## 73. Phase 8 — Extension et optimisation

1. Ajouter progressivement des secteurs.
2. Étendre la population.
3. Mesurer puis optimiser l'IA.
4. Évaluer le besoin ECS et déplacer uniquement les traitements nécessaires.
5. Déployer météo, cycle jour/nuit, saison si prévue.
6. Compléter l'audio, l'UI, les options et la localisation.

**Validation** : budgets mesurés dans les scènes les plus coûteuses ; aucune extension ne casse le parcours pilote.

## 74. Phase 9 — Préproduction finale et release

1. Valider tout le contenu.
2. Résoudre les IDs, dialogues et références manquants.
3. Effectuer tests fonctionnels, sauvegardes, streaming et régression.
4. Auditer l'accessibilité et les performances.
5. Vérifier l'absence d'outils développeur en Release.
6. Tester une installation propre.
7. Archiver les builds et les rapports.

**Validation** : un parcours complet du jeu est reproductible à partir d'une installation propre et d'une nouvelle sauvegarde.

---

# TOME XIV — ARBORESCENCE CIBLE DU PROJET

```text
Assets/_ValVertII/
├── Art/
│   ├── Characters/{Karim,Teacher,Father,Residents}/
│   ├── Buildings/
│   ├── Environment/{ValVert,Seynod,Vegetation,Water}/
│   ├── Props/
│   ├── Materials/
│   ├── Textures/
│   ├── Lighting/
│   └── VFX/
├── Audio/
│   ├── Ambience/
│   ├── Characters/
│   ├── Dialogue/
│   ├── Environment/
│   ├── Phone/
│   ├── UI/
│   └── Mixers/
├── Data/
│   ├── Characters/
│   ├── Residents/
│   ├── Items/
│   ├── Activities/
│   ├── Schedules/
│   ├── Dialogues/
│   ├── NarrativeEvents/
│   ├── Relationships/
│   ├── Locations/
│   ├── Sectors/
│   └── Settings/
├── Developer/
│   ├── Runtime/
│   ├── Editor/
│   └── Settings/
├── Inputs/
├── Prefabs/
│   ├── Player/
│   ├── Characters/
│   ├── Residents/
│   ├── Interactables/
│   ├── Doors/
│   ├── Seats/
│   ├── Buildings/
│   ├── Props/
│   └── UI/
├── Scenes/
│   ├── Bootstrap/
│   ├── Menus/
│   ├── World/
│   ├── Sectors/
│   ├── Interiors/
│   └── Tests/
├── Scripts/
│   ├── Core/{Bootstrap,Events,Services,Diagnostics}/
│   ├── Input/
│   ├── Player/{Movement,Camera,Animation,State}/
│   ├── Characters/
│   ├── Interaction/
│   ├── Phone/
│   ├── Dialogue/
│   ├── Narrative/
│   ├── Relationships/
│   ├── Memory/
│   ├── Knowledge/
│   ├── Activities/
│   ├── Schedules/
│   ├── Inventory/
│   ├── AI/{Authoring,Navigation,Simulation}/
│   ├── ECS/
│   ├── World/{Seynod,ValVert,Sectors,Streaming,Interiors}/
│   ├── Time/
│   ├── Weather/
│   ├── Save/
│   ├── Audio/
│   ├── UI/
│   └── Build/
├── Settings/{Rendering,Audio,Input,Camera,Build}/
├── Tests/{EditMode,PlayMode,Integration,Performance}/
└── Editor/{Inspectors,Windows,Validators,Importers}/
```

Les noms d'assemblies doivent être cohérents : `ValVertII.Core`, `ValVertII.Input`, `ValVertII.Player`, `ValVertII.Narrative`, `ValVertII.World`, `ValVertII.Save`, `ValVertII.AI`, `ValVertII.ECS`, `ValVertII.Developer` et `ValVertII.Tests`. L'assembly Editor est marquée Editor-only et ne doit pas être référencée par le runtime Release.

---

# TOME XV — MATRICE DE VALIDATION ET DÉCISIONS

## 75. Matrice de traçabilité

| Domaine | Preuve de réalisation attendue |
|---|---|
| Projet et packages | Projet ouvre, packages verrouillés, compilation propre, build test. |
| Bootstrap | Démarrage déterministe, aucun service dupliqué. |
| Entrées | Clavier, souris, manette, changements de maps sans double commande. |
| Personnage | Déplacement, collision, pente, escalier, accroupissement, assise. |
| Caméras | POV, troisième personne, changement, collision caméra, cinématique. |
| Mode développeur | Vol, téléportation, coordonnées, reprise du joueur, absent de Release. |
| Personnages jouables | État distinct, transitions narratives, reprise cohérente. |
| Interactions | Portes, bancs, objets, personnes, conditions et résultats. |
| Téléphone | Appel, messages, réponses, événements et persistance. |
| Dialogues | Conditions, choix, transitions, localisation et conséquences. |
| Relations/mémoire | Vérité vs perception, partage d'information, oubli et persistance. |
| IA/routines | Emploi du temps, navigation, activités de repli, simulation à distance. |
| ECS | Prototype mesuré et synchronisation correcte si ECS retenu. |
| Monde | Repères géographiques, échelle, collisions et navigation. |
| Streaming | Chargement, déchargement, restauration, anti-duplication et état stable. |
| Temps/météo | Horloge, pause, rattrapage hors écran, transitions météo. |
| Sauvegarde | Écriture atomique, backup, checksum, migration, récupération. |
| Audio/visuels/UI | Ambiances spatialisées, performances, accessibilité. |
| Outils | Validation d'IDs, secteurs, dialogues, navigation et données. |
| QA/release | Tests réussis, build reproductible, absence d'outils développeur. |

## 76. Risques principaux

| Risque | Prévention |
|---|---|
| Périmètre trop vaste | Prototype vertical et secteurs progressifs. |
| Simulation trop coûteuse | Niveaux de simulation, mesures, ECS uniquement si utile. |
| IA omnisciente | Séparer événement, perception, connaissance et interprétation. |
| Perte de données | DTO versionnés, écritures atomiques, backup et tests de restauration. |
| États oubliés après streaming | IDs stables et persistance indépendante des prefabs. |
| Conflits de contrôle | Input router unique et modes exclusifs. |
| Caméras qui perturbent le gameplay | Une caméra Unity de rendu, rigs Cinemachine et tests dédiés. |
| Contenu géographique non réutilisable | Vérifier licences et provenance avant intégration. |
| Régression de packages | Versions verrouillées et mises à jour dédiées. |
| Outils internes dans la version publique | Exclusion compile-time + test automatisé du build. |

## 77. Décisions restant à confirmer pendant la préproduction

Ces décisions n'empêchent pas de commencer le socle, mais elles doivent être fixées avant de produire les contenus concernés :

1. Plateforme de livraison initiale et configuration PC minimale.
2. Langue(s) de sortie et besoin de doublage intégral ou partiel.
3. Périmètre exact du plan de Seynod et des bâtiments accessibles.
4. Source géographique retenue et droits de réutilisation.
5. Vitesse du temps de jeu et durée d'une journée.
6. Activités quotidiennes indispensables au premier parcours.
7. Niveau de détail du téléphone et des messages.
8. Règles narratives précises du changement de protagoniste.
9. Taille cible de population proche, intermédiaire et lointaine, après profilage.
10. Usage final d'ECS et d'Addressables, après prototype et mesure.
11. Vélo, transports publics ou véhicules : inclusion dans le périmètre initial ou extension ultérieure.
12. Sauvegardes multiples, autosave et éventuelle synchronisation cloud.

## 78. Règle de vérité sur l'état du projet

Une fonctionnalité passe par les états suivants :

- **Spécifiée** : comportement, données, dépendances et tests décrits.
- **Implémentée** : code et assets intégrés dans le projet.
- **Intégrée** : connectée aux autres systèmes dans une scène réelle.
- **Testée** : tests automatisés et manuels exécutés.
- **Validée** : critères d'acceptation atteints, preuve enregistrée et aucune anomalie bloquante connue.

Un script fourni dans ce manuel est un exemple ou un contrat d'implémentation tant qu'il n'a pas été compilé et testé dans le projet Unity réel.

---

# ANNEXE A — CORRECTIONS APPORTÉES AUX SOURCES FUSIONNÉES

1. Les références à Aetheria sont supprimées : nom, namespaces, dossiers, assets et exemples sont renommés ValVertII.
2. Les modules de combat, ennemis, dégâts, santé, projectiles, butin de combat et quêtes Kill/Collect sont supprimés du périmètre.
3. Les systèmes réutilisables du tutoriel générique (bootstrap, scènes, événements, input, ECS, streaming, sauvegarde, profiling, outils) sont réaffectés aux besoins de simulation sociale.
4. La version de référence est fixée à Unity 6.6 LTS, et non à une version Unity 2022 ou à un assemblage de packages anciens.
5. Le prototype FreeFlyCamera fondé sur `UnityEngine.Input` est remplacé par une map Developer du New Input System.
6. La caméra de jeu n'est pas simplement désactivée par `SetActive` : le système central gère le CinemachineBrain, le rig actif, le curseur et les maps d'entrée.
7. Les deux caméras de rendu ne sont pas activées simultanément : une caméra Unity de rendu + CinemachineBrain est la cible.
8. Le déplacement ECS direct par translation de Transform n'est pas considéré comme une navigation de production : le chemin doit respecter NavMesh ou graphe de routes.
9. La vérité du monde est séparée de la connaissance et de l'interprétation de chaque habitant.
10. Les sauvegardes ne sérialisent pas des références Unity directes ; elles utilisent des DTO versionnés et des identifiants stables.
11. Le temps du monde est indépendant de `Time.timeScale`.
12. L'usage d'ECS, d'Addressables, du pooling ou d'autres technologies est conditionné par un besoin mesuré, pas par la seule ambition du projet.
13. Les valeurs de déplacement, les vitesses de caméra et les volumes de population sont des valeurs initiales à calibrer, non des résultats validés.
14. Le mode développeur est distinct du gameplay, protégé par compilation et absent de la version Release.

# ANNEXE B — RÉFÉRENCES TECHNIQUES OFFICIELLES

Références consultées pour établir la base technique au 7 novembre 2026. Les versions de packages peuvent évoluer ; vérifier à nouveau la matrice de compatibilité au moment de créer ou mettre à jour le projet.

- Unity — politique de support Unity 6 et calendrier LTS : https://unity.com/fr/releases/unity-6/support
- Unity — Unity 6.6 LTS disponible : https://unity.com/blog/unity-6-3-lts-is-now-available
- Unity Docs — liste des packages publiés pour l'éditeur 6000.3 : https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe
- Input System 1.20.1 publié pour Unity 6000.3 : https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe/com-unity-inputsystem
- Cinemachine 3.1.7 publié pour Unity 6000.3 : https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe/com-unity-cinemachine
- Entities 1.5.0 publié pour Unity 6000.3 : https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe/com-unity-entities
- URP — package core lié à la version de l'éditeur et compatibilité : https://docs.unity.com/en-us/engine/6000.3/manual/render-pipelines/universal-render-pipeline/requirements

---

# CONCLUSION — DÉFINITION DE « VAL VERT II EST PRÊT »

Val Vert II n'est pas prêt parce que ses dossiers existent, que les scripts compilent ou que la caméra se déplace dans une scène vide. Il est prêt lorsque son parcours principal est jouable de bout en bout, que les personnages poursuivent des activités crédibles, que les informations circulent avec des règles cohérentes, que les lieux sont traversables, que les sauvegardes survivent aux mises à jour prévues, que le streaming restaure l'état du monde, que les performances sont mesurées et que le build public ne contient aucun outil interne.

Le premier objectif concret est une tranche verticale : Karim dans une petite portion reconnaissable du bois du Val Vert, un déplacement propre, une interaction avec un banc, un appel de sa mère, un choix, une réaction différée, une sauvegarde, un rechargement et un habitant qui connaît uniquement les faits qu'il a pu apprendre. Une fois cette tranche entièrement validée, le reste du monde peut être construit sur des fondations vérifiées.

**Ce manuel constitue la référence consolidée de conception et de réalisation. Il décrit ce qu'il faut construire et comment le valider ; il ne déclare pas fictivement les systèmes déjà terminés.**
