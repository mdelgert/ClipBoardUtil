#include <QCoreApplication>
#include <QClipboard>
#include <QGuiApplication>
#include <QTimer>
#include <iostream>

class ClipboardManager : public QObject
{
    Q_OBJECT

public:
    ClipboardManager(QObject* parent = nullptr) : QObject(parent)
    {
        clipboard = QGuiApplication::clipboard();
        connect(clipboard, &QClipboard::dataChanged, this, &ClipboardManager::onClipboardChanged);
    }

private slots:
    void onClipboardChanged()
    {
        QString text = clipboard->text();
        std::cout << "Clipboard content changed: " << text.toStdString() << std::endl;
    }

private:
    QClipboard* clipboard;
};

int main(int argc, char *argv[])
{
    QCoreApplication app(argc, argv);

    ClipboardManager manager;

    return app.exec();
}

#include "main.moc"
