 # Guardian of the Light
Лучшая игра-платформер 2019 года!

## TODO list && Roadmap
- огонь в светильнике не всегода корректно отображается поэтому необходимо переделать скрипт  Camera Bildoard Ex - так что бы он поворачивал обьект к  камере c с тегом PerspectiveVievCamera
- Убрать взаимодействие с бутылкой в режиме инвентаря( бутылка будет отображать лишь текущее HP внутри не, а опустошить ее или наполнить  можно в режиме просмотра обьекта

### Инвентарь
- при зажатии кнопок A D  свет выделения предмета необходимо перемешаться по ячейкам, тоесть если я зажал кнопку свет переходт на одну потом на следующую и т.д пока ты не отпустил кнопку, вот таакое предложение

- Сделать так что бы поле для текста в скрипте инвенторя увеличивавалось с количеством символов ( это уже делалали но куда то потерялось)
- Исправить перелистование объектов в инвентаре, когда в инвентаре предметом больше 5

### Игрок
- правки по герою и грибу - гриб работает отлично за исключением того что надо реализовать возврат здоровья в него так же с возможностью назначение кнопки, сейчас предлагаю  вариан  где  запускаеется анимация и всплывает окошко где написано  что если нажать на J( забрать, на I отдать свет) (L) в данном случае будет выходом из окна по визуальному ряду думаю будет похоже на то как в скайриме ты работаешь в кузнице.
- Пресеты материалов работают отлично, но  игрок состоит из 2ух мешей где второй это лицо и глаза, на него свой материал а следоватьельно нужно еще 2 пресета для лица игрока, иначе глаза будут сильно выделяться из общей картины так как их свечение не меняется.

- Реализовать IK так что бы игрок ходил по мешам
- Реализовать перемещение игрока по локациям с сохранением его уровня здоровья и предметов в инвенторе, в общем с сохранением всего

### Игровые объекты
- Сделать так что бы дверь открывалась всегда вперед??? это пока под вопросом в виду того что пока не ясно как будет выглядеть анимация героя

### Мир

* Добавить возможность открывания всех дверей в лесу и шахте
* Добавить возможность перехода между локациями (4 портала) 
* Добавить эффект перехода в другую локацию
* Добавить подсказку подтверждения перехода в другую локацию с её именем
* Добавить анимацию поднимания предметов (бутылка, шестеренка и далее) на слое UI

