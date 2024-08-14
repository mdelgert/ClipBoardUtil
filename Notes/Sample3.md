import pyperclip
import time

def monitor_clipboard(interval=0.5):
    last_text = pyperclip.paste()

    while True:
        current_text = pyperclip.paste()
        if current_text != last_text:
            print(f"Clipboard updated: {current_text}")
            last_text = current_text
        
        time.sleep(interval)

if __name__ == "__main__":
    print("Starting clipboard monitor...")
    monitor_clipboard()
