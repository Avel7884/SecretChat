# SecretChat
Обёртка над [Вконтакте](https://vk.com/) для зашифрованой переписки с людьми.

_Название проекта SecretChat выбрано по аналогии с секретными чатами в телеграме, чтобы пользователи могли зашифрованными сообщениями даже если телеграм будет заблакирован_

Авторы: Щетников Павел, Ханова Анна

# Черновой план презентации защиты:

## Описание сути проекта
Мы сделали приложение-обёртку над мессенджером, позволяющее шифровать и обмениваться дешифровать сообщениями.
На данный момент реализована обёртка над [Вконтакте](https://vk.com/), сообщения шифруются с помощью [one-time pad (OTP)](https://en.wikipedia.org/wiki/One-time_pad) шифрования, используется простой консольный интерфейс.
В разработке находится графический интерфейс.

Точки расширения:
- Основная точка расширения --- различные мессенджеры, различные шифрования.
Архитектура уровня Domain написана так, чтобы добавить новое шифрование или новый месенджер было как можно проще.

- Можно создавать различные классы диалогов со сколь угодно подробным содержанием сообщений. От просто текстовых до содержащих Attachment, стикер, emoji.

- Возможно, к защите проекта будет готова точка расширения, позволяющая позволяющая создавать различные стили графического интерфейса.

Павел занимался получением информации об объектах с помощью VkApi, а также реализацией интерфейса, позволяющего легко добавлять другие мессенджеры. Анна работали над реализацией шифрования, консольным интерфейсом и тестированием.

## Описание точек расширения
### Разные виды мессенджеров
В основе абстракции лежат интерфейсы [IConnecter](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IConnecter.cs) и [IDialog](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IDialog.cs). IConnecter является фабрикой IDialog. Его задача установить соединение и создать диалог, в котором будут находиться указанные пользователи. IDialog имеет методы getMessages - получить непрочитанные ранее сообщения из этого диалога, sendMessage -- отправить сообщение. Эти методы не должны поддерживать шифрований.
Чтобы добавить новый мессенджер - нужно написать реализацию этих интерфейсов.
Также, если для удобного получения информации от мессенджера подразумевается работа с API, нужно реализовать интерфейс [IApiRequests](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IApiRequests.cs). А чтобы получать больше информации о сообщениях, например, имя отправителя, нужно реализовать интерфейс [IUserManager](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IUsersManager.cs).
Однако, если мы не хотим добавить новый мессенджер, а сделать другую реализацию работы с Vk, то можно будет переиспользовать значительную часть логики более низких уровней.

### Добавление новых видов шифрований.
Нужно реализовать абстрактный класс [MessageStream](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/MessageEncryption/IMessageStream.cs), в него надо передать потоки, через которые пользователь будет взаимодействовать с приложением, а также методы ReadMessage и WriteMessage, которые будут читать/писать сообщения потоки и декодировать/кодировать, в соответствии с шифрованием.
Также, если для шифрования используется ключ, нужно реализовать интерфейс [IKeyReader](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Infrastructure/IKeyReader.cs), он знает как получить ключ возвращает следующие count байт ключа. 
  
## Структура решения.

  - В Infrastructure реализованы вспомогательные классы например [KeyReader](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Infrastructure/FileKeyReader.cs), который используется для получения ключа.

  - Логика приложения собрана в Domain. 
  -- Логика работы с мессенджером находится в Domain.InteractionWithSomeMessanger
  Здесь есть классы и интерфейсы различного уровня абстракции,
   от подходящего любому мессенджеру с диалогами и залогиниванием до собственно реализации работы с VK.
  Самый низкий уровень абстракции - это классы [AbstractInteractionWithMessanger](https://github.com/Avel7884/SecretChat/tree/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger)
  Далее по иерархии наследования идёт [AbstractVkInteraction](https://github.com/Avel7884/SecretChat/tree/master/SecretChat/Domain/InteractionWithSomeMessanger/InteractionWithVk/AbstractVkInteraction), данные интерфейсы предполагают, что будут взаимодействовать именно с Vk.

  Ну и на последнем уровне абстракции - собственно [CustomVkInteraction](https://github.com/Avel7884/SecretChat/tree/master/SecretChat/Domain/InteractionWithSomeMessanger/InteractionWithVk/CustomVkInteraction) или какие-то другие классы, позволяющие обмениваться сообщениями в мессенджере.
  
  - В Application находятся классы-прослойки между классами уровня Domain и UserInterface.
    Они реализуют различную логику несвязанную напрямую с игрой.
    Например, класс [ChessAlikeApp](https://github.com/Tinsane/WarChess/blob/master/WarChess/Application/ChessAlikeApp.cs)
    реализует логику, связанную с выделением фигуры, которой будет производится ход.
    
  - Наконец, в UserInterface собраны все формы, контролы и прочие вещи, необходимые для визуализации.
    Во время реализации классов UserInterface мы старались делать Control-ы максимально глупыми.
    Это позволяет переиспользовать одни и те же контролы для разных игр.
    Например, контрол для шахматного поля [BoardControl](https://github.com/Tinsane/WarChess/blob/master/WarChess/UserInterface/BoardControl.cs) не знает ничего ни о каких шахматах. 
    Ему просто передают картинки и он их рисует.
    Также интересен [ChessAlikeGameControl](https://github.com/Tinsane/WarChess/blob/master/WarChess/UserInterface/ChessAlikeGameControl.cs), который принимает большое количество интерфейсов и реализует логику игры для произвольного IChessAlikeGame.
  
## DI-КОНТЕЙНЕР.
Интерфайс [IConnecter](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IConnecter.cs) является фабрикой, поэтому [IDialog](https://github.com/Avel7884/SecretChat/blob/master/SecretChat/Domain/InteractionWithSomeMessanger/AbstractInteractionWithMessanger/IDialog.cs) собирается внутри него при помощи конвенций.
Остальное - явным конфигурированием.
Синглтонами сделаны класс работы с API, класс взаимодействия с пользователем, 
Также синглтоном является класс VKUsersManager, поскольку его логика всегда одинаковая.

## Тесты
Тестами пока покрыта логика шифрующая сообщения.
Проверяется корректность работы с длинными текстами, с кириллицей, пробелами, переводами строк и знаками препинания. Также, проверяется корректность работы с некоторыми алфавитами такими как японский, китайский упрощённый, арабский.
Для тестирования использовалась библиотека NUnit.
Логика уровня приложения оказалась очень простой, поэтому тестами мы её решили не покрывать.
