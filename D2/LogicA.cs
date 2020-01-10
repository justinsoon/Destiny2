using System.IO;

namespace BunnyKompilar
{
    internal static partial class Logic0
    {

        internal static string TheMaoci = @"; -- XenosMaoci
#NoEnv
#Persistent
#SingleInstance, Force
#KeyHistory, 0
#InstallKeybdHook
#InstallMouseHook
#UseHook
#HotKeyInterval 1 
#MaxHotkeysPerInterval 127
SetBatchLines,-1
SetKeyDelay,-1, 8
SetControlDelay, -1
SetMouseDelay, 0
SetWinDelay,0
ListLines, Off
Coordmode, mouse, screen
;--- Starting from Pure Statics --- or safe variables
aimL := 1
aimR := 1
mute := 0
PMode := 0
do_recoil := 0
b := 0
; --- not used --- maybe later ?
aimC := 0 ; custom
customkey := ""Not Set"" ; Any single key or mouse button
DBG := 0 ; for gui now no gui aviable
; ------
iniFileName := ""config.ini""
redName := ""1.bmp""
whiName := ""2.bmp""
pveName := ""3.bmp""
pveBossName := ""orangehealthbar.bmp""

; Check existance of bmp and ini files
if(!FileExist(""1.bmp"")){
redName := ""bm1.bmp""
}
if(!FileExist(""2.bmp"")){
whiName := ""bm2.bmp""
}
if(!FileExist(""3.bmp"")){
pveName := ""bm3.bmp""
}
if(!FileExist(""orangehealthbar.bmp"")){
pveBossName := ""bmorangehealthbar.bmp""
}
if(!FileExist(iniFileName)){
FileAppend, 
(
[Aim]
speed=13.5
antishake=0
ScanX=32
ScanY=30
move1=10
move2=20
Loop=4
Redc=2
Timeout=1
pm=0
[Position]
OffX1=68
OffY1=52
[dara]
AbsMin=20
[Rdata]
R1t=45
R2t=50
R3t=50
[Wdata]
Ww1=1
Wh=4
Wt=15
[Gdata]
Gw=1
Gh=4
Gt=15
[acti]
keyL=0
keyR=1
[KEYS]
hk1=F5
hk2=F6
hk3=F7
hk4=F8
hk5=F9
hk6=F11
), %iniFileName%
}
gosub, LoadINI ; Load INI + one time calculations
; [hotkeys]
hk_1 = %hk1% ; main on off
hk_2 = %hk2% ; not used
hk_3 = %hk3% ; not used
hk_4 = %hk4% ; Load New INI
hk_5 = %hk5% ; Debug - in development - not added
hk_6 = %hk6% ; Terminate

; [ Globals / Statics - scaling to 1920x1080 ]
g_W_dt := A_Screenwidth / 2 ; center for width
g_H_dt := A_Screenheight / 2 ; center for height
g_W_tv := (A_Screenwidth * 110 / 1920) ; tunel vision W
g_H_tv := (A_Screenheight * 90 / 1080) ; tunel vision H
g_rw1 := Floor(A_Screenwidth * 140 / 1920) ; max red W
g_rh1 := Floor(A_Screenheight * 6 / 1080) ; min red H
g_rw2 := Floor(A_Screenwidth * 90 / 1920) ; mid red W
g_rh2 := Floor(A_Screenheight * 8 / 1080) ; mid red H
g_rw3 := Floor(A_Screenwidth * 30 / 1920) ; min red W
g_rh3 := Floor(A_Screenheight * 9 / 1080) ; max red H
; --- [ set scan area ] ---
GoSub, g_scan
;--- [ Hotkey assingns ] ---
; HOTKEY setup
loop, 6 {
	if (hk_%A_Index% != """" && hk_%A_Index% != ""Not set"") {
		Hotkey, % hk_%A_Index%, f_hk%A_Index%
	}
}
; -------------------- MAIN BOT LOOP ----------------------
loop, {	
	if (b = 1) {
		if ((GetKeyState(""RButton"", ""P"")) && (aimR = 1)) 
		|| ((GetKeyState(""LButton"", ""P"")) && (aimL = 1))
		{
			if (lockOn = """") {
				GoSub, findTarget
			} else {
				GoSub, lockTarget
			}
			continue
		} 
		if (ai = 1) {
			gosub, g_scan
		}
	}
    Sleep, 1
}
return

; ------ [ LABELS ] ------
; --- [ activate normal scan ] ---
g_scan:
ai := 0
X1 := ScanAreaX1
Y1 := ScanAreaY1
X2 := ScanAreaX2
Y2 := ScanAreaY2
return

; --- [ activate tunel vision ] ---
ai_scan:
ai := 1
X1 := aX - g_W_tv
X2 := aX + g_rw1 + g_W_tv
Y1 := aY - g_H_tv
Y2 := aY + g_rh2 + g_H_tv
return
; --- [ unlocking after timeout ] ---
ai_timeout:
timeout := 1
lockOn := """"
retry := 0
return
; --- [ searching for enemy ] ---
findTarget:
loop, 3 {
	i := A_Index
	v1 := ""Red"" . i . ""t"",v1:=%v1%
	v2 := ""g_rw"" . i,v2:=%v2%
	v3 := ""g_rh"" . i,v3:=%v3%
	imagesearch, aX, aY, %X1%, %Y1%, %X2%, %Y2%, *%v1% *w%v2% *h%v3% %redName%
	if (ErrorLevel = 0) {
			GoSub, scanB1
			if (ErrorLevel != 0) {
				return
			}
		GoSub, mouse1
		lockOn := i
		retry := 0
		GoSub, ai_scan
		settimer, ai_timeout, -%t%
		return
	}
}
return
; --- [ target acuired ] ---
lockTarget: ; On Locked scan
imagesearch, aX, aY, %X1%, %Y1%, %X2%, %Y2%, *%v1% *w%v2% *h%v3% %redName%
	if (ErrorLevel = 0) {
			GoSub, scanB1
			if (ErrorLevel != 0) {
				return
			}
		GoSub, mouse1
		lockOn := i
		gosub, ai_scan
		settimer, ai_timeout, -%t%
		return
	}
		gosub, g_scan
	if (lockOn = 1) {
		retry := 0
		lockOn := """"
		return
	}
	if (v2 > g_minv) {
		v2 := Floor(v2 * (100 - reduc) / 100) 
	}
	if (retry < loops) {
		retry ++
	} else {
		retry := 0
		lockOn := """"
	}
	if (timeout = 1) {
		timeout := 0
		retry := 0
		lockOn := """"
	}
return
; ---[ white level/pve font search ]---
scanB1: 
if(PMode == false){
newX1 := aX - Floor(A_Screenwidth * 38 / 1920)
newX2 := aX - Floor(A_Screenwidth * 8 / 1920)
newY1 := aY - Floor(A_Screenheight * 22 / 1080)
newY2 := aY + Floor(A_Screenheight * 10 / 1080)
imagesearch, newX, newY, %newX1%, %newY1%, %newX2%, %newY2%, *%WhiteT% *w%WhiteW% *h%WhiteH% %whiName%
}
if(PMode == true){
newX1 := aX - Floor(A_Screenwidth * 3 / 1920)
newX2 := aX + Floor(A_Screenwidth * 30 / 1920)
newY1 := aY - Floor(A_Screenheight * 23 / 1080)
newY2 := aY - Floor(A_Screenheight * 3 / 1080)
imagesearch, newX, newY, %newX1%, %newY1%, %newX2%, %newY2%, *%GreyT% *w%GreyW% *h%GreyH% %pveName% %pveBossName%
}
return
; --- [ calc moving and move ] ---
mouse1: 
AimX := aX - g_W_dt + u_aimX1
AimY := aY - g_H_dt + u_aimY1
if (AimX > shake) {
	DirX := Scan2P
	mulX := 1.05
} else if (AimX > 0) {
	DirX := Scan1P
	mulX := 1
} else if (AimX < -shake) {
	DirX := Scan2N
	mulX := 1.05
} else if (AimX < 0) {
	DirX := Scan1N
	mulX := 1
}
if (AimY > shake) {
	DirY := Scan2P
	mulY := 1.02
} else if (AimY > 0) {
	DirY := Scan1P
	mulY := 1
} else if (AimY < -shake) {
	DirY := Scan2N
	mulY := 1.02
} else if (AimY < 0) {
	DirY := Scan1N
	mulY := 1
}
MoveX := Ceil( AimX * mulX * DirX) * DirX
MoveY := Ceil( AimY * mulY * DirY) * DirY
;MoveX := Ceil( AimX )
;MoveY := Ceil( AimY )
DllCall(""mouse_event"", uint, 1, int, MoveX, int, MoveY, uint, 0, int, 0) ; mouse move
return

LoadINI:
iniread,sen,%iniFileName%,Aim,speed
iniread,shake,%iniFileName%,Aim,antishake
iniread,scanX,%iniFileName%,Aim,ScanX
iniread,scanY,%iniFileName%,Aim,ScanY
iniread,antiX,%iniFileName%,Aim,AntiRcX
iniread,antiY,%iniFileName%,Aim,AntiRcY
iniread,loops,%iniFileName%,Aim,Loop
iniread,reduc,%iniFileName%,Aim,Redc
iniread,timeout,%iniFileName%,Aim,Timeout
iniread,move1,%iniFileName%,Aim,move1
iniread,move2,%iniFileName%,Aim,move2
iniread,PMode,%iniFileName%,Aim,pm
iniread,u_aimX1,%iniFileName%,Position,OffX1
iniread,u_aimY1,%iniFileName%,Position,OffY1
iniread,abs_min,%iniFileName%,dara,AbsMin
iniread,Red1t,%iniFileName%,Rdata,R1t
iniread,Red2t,%iniFileName%,Rdata,R2t
iniread,Red3t,%iniFileName%,Rdata,R3t
iniread,WhiteW,%iniFileName%,Wdata,Ww1
iniread,WhiteH,%iniFileName%,Wdata,Wh
iniread,WhiteT,%iniFileName%,Wdata,Wt
iniread,GreyW,%iniFileName%,Gdata,Gw
iniread,GreyH,%iniFileName%,Gdata,Gh
iniread,GreyT,%iniFileName%,Gdata,Gt
iniread,aimL,%iniFileName%,acti,keyL
iniread,aimR,%iniFileName%,acti,keyR
iniread,hk1,%iniFileName%,KEYS,hk1
iniread,hk2,%iniFileName%,KEYS,hk2
iniread,hk3,%iniFileName%,KEYS,hk3
iniread,hk4,%iniFileName%,KEYS,hk4
iniread,hk5,%iniFileName%,KEYS,hk5
iniread,hk6,%iniFileName%,KEYS,hk6
; -----[ Recalculating staff ]-----
Scan2P := sen / move2
Scan1P := sen / move1
Scan2N := (-sen) / move2
Scan1N := (-sen) / move1
t := 0 + timeout * 1000
g_minv := Floor(A_Screenwidth * (abs_min / 100) * 150 / 1920)
ScanAreaX1 := 0 + (A_Screenwidth * (scanX / 100))
ScanAreaY1 := 0 + (A_Screenheight * (scanY / 100))
ScanAreaX2 := A_Screenwidth - (A_Screenwidth * (scanX / 100))
ScanAreaY2 := A_Screenheight - (A_Screenheight * (scanY / 100))
gosub, g_scan

return

; -----[ Hotkeys assigns ]-----
f_hk1: ; master on / off
Keywait, %hk_1%, U
lockOn := """"
b := !b
l := 1 + b
if (mute = 0) {
	loop, %l% {
		Soundbeep, 900, 100
	}
}
return

f_hk2: ; Change Mode
Keywait, %hk_2%, Up
PMode := !PMode
if(PMode == false)
Soundbeep, 1000, 200
if(PMode == true)
Soundbeep, 250, 200
return

f_hk3:
Keywait, %hk_3%, Up
; ---[ empty one ]---
return
f_hk4: ; Load Changed INI
Keywait, %hk_4%, Up
if (mute = 0) {
		gosub, LoadINI
		loop, 2 {
			Soundbeep, 1000, 50
		}
}
return
f_hk5: ; Debug on / off
Keywait, %hk_5%, Up
if (mute = 0) {
		DBG := !DBG
		loop, 1 {
			Soundbeep, 1000, 50
		}
}
return
f_hk6: ; Terminate
Keywait, %hk_6%, Up
ExitApp
";
    }
}
