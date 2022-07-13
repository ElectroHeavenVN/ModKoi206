using System;

public class ChatPopup : Effect2, IActionListener
{
	public int sayWidth = 100;

	public int delay;

	public int sayRun;

	public string[] says;

	public int cx;

	public int cy;

	public int ch;

	public int cmx;

	public int cmy;

	public int lim;

	public Npc c;

	private bool outSide;

	public static long curr;

	public static long last;

	private int currentLine;

	private string[] lines;

	public Command cmdNextLine;

	public Command cmdMsg1;

	public Command cmdMsg2;

	public static ChatPopup currChatPopup;

	public static ChatPopup serverChatPopUp;

	public static string nextMultiChatPopUp;

	public static Npc nextChar;

	public bool isShopDetail;

	public sbyte starSlot;

	public sbyte maxStarSlot;

	public static Scroll scr;

	public static bool isHavePetNpc;

	public int mH;

	public static int performDelay;

	public int dx;

	public int dy;

	public int second;

	public int strY;

	private int iconID;

	public static void addNextPopUpMultiLine(string strNext, Npc next)
	{
		nextMultiChatPopUp = strNext;
		nextChar = next;
		if (currChatPopup == null)
		{
			addChatPopupMultiLine(nextMultiChatPopUp, 100000, nextChar);
			nextMultiChatPopUp = null;
			nextChar = null;
		}
	}

	public static void addBigMessage(string chat, int howLong, Npc c)
	{
		GameScr.info1.addInfo(chat, 0);
	}

	public static void addChatPopupMultiLine(string chat, int howLong, Npc c)
	{
		GameScr.info1.addInfo(chat, 0);
	}

	public static ChatPopup addChatPopupWithIcon(string chat, int howLong, Npc c, int idIcon)
	{
		performDelay = 10;
		ChatPopup chatPopup = new ChatPopup();
		chatPopup.sayWidth = GameCanvas.w - 30 - (GameCanvas.menu.showMenu ? GameCanvas.menu.menuX : 0);
		if (chatPopup.sayWidth > 320)
			chatPopup.sayWidth = 320;
		if (chat.Length < 10)
			chatPopup.sayWidth = 64;
		if (GameCanvas.w == 128)
			chatPopup.sayWidth = 128;
		chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
		chatPopup.delay = howLong;
		chatPopup.c = c;
		chatPopup.iconID = idIcon;
		Char.chatPopup = chatPopup;
		chatPopup.ch = 15 - chatPopup.sayRun + chatPopup.says.Length * 12 + 10;
		if (chatPopup.ch > GameCanvas.h - 80)
			chatPopup.ch = GameCanvas.h - 80;
		chatPopup.mH = 10;
		if (GameCanvas.menu.showMenu)
			chatPopup.mH = 0;
		Effect2.vEffect2.addElement(chatPopup);
		isHavePetNpc = false;
		if (c != null && c.charID == 5)
		{
			isHavePetNpc = true;
			GameScr.info1.addInfo(string.Empty, 1);
		}
		curr = (last = mSystem.currentTimeMillis());
		chatPopup.ch += 15;
		return chatPopup;
	}

	public static ChatPopup addChatPopup(string chat, int howLong, Npc c)
	{
		performDelay = 10;
		ChatPopup chatPopup = new ChatPopup();
		chatPopup.sayWidth = GameCanvas.w - 30 - (GameCanvas.menu.showMenu ? GameCanvas.menu.menuX : 0);
		if (chatPopup.sayWidth > 320)
			chatPopup.sayWidth = 320;
		if (chat.Length < 10)
			chatPopup.sayWidth = 64;
		if (GameCanvas.w == 128)
			chatPopup.sayWidth = 128;
		chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
		chatPopup.delay = howLong;
		chatPopup.c = c;
		Char.chatPopup = chatPopup;
		chatPopup.ch = 15 - chatPopup.sayRun + chatPopup.says.Length * 12 + 10;
		if (chatPopup.ch > GameCanvas.h - 80)
			chatPopup.ch = GameCanvas.h - 80;
		chatPopup.mH = 10;
		if (GameCanvas.menu.showMenu)
			chatPopup.mH = 0;
		Effect2.vEffect2.addElement(chatPopup);
		isHavePetNpc = false;
		if (c != null && c.charID == 5)
		{
			isHavePetNpc = true;
			GameScr.info1.addInfo(string.Empty, 1);
		}
		curr = (last = mSystem.currentTimeMillis());
		return chatPopup;
	}

	public static void addChatPopup(string chat, int howLong, int x, int y)
	{
		GameScr.info1.addInfo(chat, 0);
	}

	public override void update()
	{
		if (scr != null)
		{
			GameScr.info1.isUpdate = false;
			scr.updatecm();
		}
		else
			GameScr.info1.isUpdate = true;
		if (GameCanvas.menu.showMenu)
		{
			strY = 0;
			cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
			cy = GameCanvas.menu.menuY - ch;
		}
		else
		{
			strY = 0;
			if (GameScr.gI().right == null && GameScr.gI().left == null && GameScr.gI().center == null && cmdNextLine == null && cmdMsg1 == null)
			{
				cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
				cy = GameCanvas.h - 5 - ch;
			}
			else
			{
				strY = 5;
				cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
				cy = GameCanvas.h - 20 - ch;
			}
		}
		if (delay > 0)
			delay--;
		if (performDelay > 0)
			performDelay--;
		if (sayRun > 1)
			sayRun--;
		if ((c != null && Char.chatPopup != null && Char.chatPopup != this) || (c != null && Char.chatPopup == null) || delay == 0)
		{
			Effect2.vEffect2Outside.removeElement(this);
			Effect2.vEffect2.removeElement(this);
		}
	}

	public override void paint(mGraphics g)
	{
		if (GameScr.gI().activeRongThan && GameScr.gI().isUseFreez)
			return;
		GameCanvas.resetTrans(g);
		int num = cx;
		int num2 = cy;
		int num3 = sayWidth + 2;
		int num4 = ch;
		if ((num <= 0 || num2 <= 0) && !GameCanvas.panel.isShow)
			return;
		PopUp.paintPopUp(g, num, num2, num3, num4, 16777215, false);
		if (c != null)
			SmallImage.drawSmallImage(g, c.avatar, cx + 14, cy, 0, StaticObj.BOTTOM_LEFT);
		if (iconID != 0)
			SmallImage.drawSmallImage(g, iconID, cx + num3 / 2, cy + ch - 15, 0, StaticObj.VCENTER_HCENTER);
		if (scr != null)
		{
			g.setClip(num, num2, num3, num4 - 16);
			g.translate(0, -scr.cmy);
		}
		int num5 = -1;
		for (int i = 0; i < says.Length; i++)
		{
			if (says[i].StartsWith("--"))
			{
				g.setColor(0);
				g.fillRect(num + 10, cy + sayRun + i * 12 + 6, num3 - 20, 1);
				continue;
			}
			mFont mFont2 = mFont.tahoma_7;
			int num6 = 2;
			string st = says[i];
			int num7 = 0;
			if (says[i].StartsWith("|"))
			{
				string[] array = Res.split(says[i], "|", 0);
				if (array.Length == 3)
					st = array[2];
				if (array.Length == 4)
				{
					st = array[3];
					num6 = int.Parse(array[2]);
				}
				num7 = int.Parse(array[1]);
				num5 = num7;
			}
			else
				num7 = num5;
			switch (num7)
			{
			case -1:
				mFont2 = mFont.tahoma_7;
				break;
			case 0:
				mFont2 = mFont.tahoma_7b_dark;
				break;
			case 1:
				mFont2 = mFont.tahoma_7b_green;
				break;
			case 2:
				mFont2 = mFont.tahoma_7b_blue;
				break;
			case 3:
				mFont2 = mFont.tahoma_7_red;
				break;
			case 4:
				mFont2 = mFont.tahoma_7_green;
				break;
			case 5:
				mFont2 = mFont.tahoma_7_blue;
				break;
			case 7:
				mFont2 = mFont.tahoma_7b_red;
				break;
			}
			if (says[i].StartsWith("<"))
			{
				string[] array2 = Res.split(Res.split(says[i], "<", 0)[1], ">", 1);
				if (second == 0)
					second = int.Parse(array2[1]);
				else
				{
					curr = mSystem.currentTimeMillis();
					if (curr - last >= 1000L)
					{
						last = curr;
						second--;
					}
				}
				mFont2.drawString(g, second + " " + array2[2], cx + sayWidth / 2, cy + sayRun + i * 12 - strY + 12, num6);
			}
			else
			{
				if (num6 == 2)
					mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY + 12, num6);
				if (num6 == 1)
					mFont2.drawString(g, st, cx + sayWidth - 5, cy + sayRun + i * 12 - strY + 12, num6);
			}
		}
		if (maxStarSlot > 0)
		{
			for (int j = 0; j < maxStarSlot; j++)
			{
				g.drawImage(Panel.imgMaxStar, num + num3 / 2 - maxStarSlot * 20 / 2 + j * 20 + mGraphics.getImageWidth(Panel.imgStar), num2 + num4 - 13, 3);
			}
		}
		if (starSlot > 0)
		{
			for (int k = 0; k < starSlot; k++)
			{
				g.drawImage(Panel.imgStar, num + num3 / 2 - maxStarSlot * 20 / 2 + k * 20 + mGraphics.getImageWidth(Panel.imgStar), num2 + num4 - 13, 3);
			}
		}
		paintCmd(g);
	}

	public void paintRada(mGraphics g, int cmyText)
	{
		int num = cx;
		int num2 = cy;
		int num3 = sayWidth;
		int num4 = 0;
		int num5 = 0;
		num4 = g.getTranslateX();
		num5 = g.getTranslateY();
		g.translate(0, -cmyText);
		if ((num <= 0 || num2 <= 0) && !GameCanvas.panel.isShow)
			return;
		int num6 = -1;
		for (int i = 0; i < says.Length; i++)
		{
			if (says[i].StartsWith("--"))
			{
				g.setColor(16777215);
				g.fillRect(num + 10, cy + sayRun + i * 12 - 6, num3 - 20, 1);
				continue;
			}
			mFont mFont2 = mFont.tahoma_7_white;
			int num7 = 2;
			string st = says[i];
			int num8 = 0;
			if (says[i].StartsWith("|"))
			{
				string[] array = Res.split(says[i], "|", 0);
				if (array.Length == 3)
					st = array[2];
				if (array.Length == 4)
				{
					st = array[3];
					num7 = int.Parse(array[2]);
				}
				num8 = int.Parse(array[1]);
				num6 = num8;
			}
			else
				num8 = num6;
			switch (num8)
			{
			case -1:
				mFont2 = mFont.tahoma_7_white;
				break;
			case 0:
				mFont2 = mFont.tahoma_7b_white;
				break;
			case 1:
				mFont2 = mFont.tahoma_7b_green;
				break;
			case 2:
				mFont2 = mFont.tahoma_7b_red;
				break;
			}
			if (says[i].StartsWith("<"))
			{
				string[] array2 = Res.split(Res.split(says[i], "<", 0)[1], ">", 1);
				if (second == 0)
					second = int.Parse(array2[1]);
				else
				{
					curr = mSystem.currentTimeMillis();
					if (curr - last >= 1000L)
					{
						last = curr;
						second--;
					}
				}
				mFont2.drawString(g, second + " " + array2[2], cx + sayWidth / 2, cy + sayRun + i * 12 - strY, num7);
			}
			else
			{
				if (num7 == 2)
					mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY, num7);
				if (num7 == 1)
					mFont2.drawString(g, st, cx + sayWidth - 5, cy + sayRun + i * 12 - strY, num7);
			}
		}
		GameCanvas.resetTrans(g);
		g.translate(num4, num5);
	}

	public void updateKey()
	{
		if (scr != null)
		{
			if (GameCanvas.isTouch)
				scr.updateKey();
			if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
			{
				scr.cmtoY -= 12;
				if (scr.cmtoY < 0)
					scr.cmtoY = 0;
			}
			if (GameCanvas.keyHold[(!Main.isPC) ? 8 : 22])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
				scr.cmtoY += 12;
				if (scr.cmtoY > scr.cmyLim)
					scr.cmtoY = scr.cmyLim;
			}
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.center))
		{
			GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
			mScreen.keyTouch = -1;
			if (cmdNextLine != null)
				cmdNextLine.performAction();
			else if (cmdMsg1 != null)
			{
				cmdMsg1.performAction();
			}
			else if (cmdMsg2 != null)
			{
				cmdMsg2.performAction();
			}
		}
		if (scr == null || !scr.pointerIsDowning)
		{
			if (cmdMsg1 != null && (GameCanvas.keyPressed[12] || GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(cmdMsg1)))
			{
				GameCanvas.keyPressed[12] = false;
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.isPointerJustRelease = false;
				cmdMsg1.performAction();
				mScreen.keyTouch = -1;
			}
			if (cmdMsg2 != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(cmdMsg2)))
			{
				GameCanvas.keyPressed[13] = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.isPointerJustRelease = false;
				cmdMsg2.performAction();
				mScreen.keyTouch = -1;
			}
		}
	}

	public void paintCmd(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		GameCanvas.paintz.paintTabSoft(g);
		if (cmdNextLine != null)
			GameCanvas.paintz.paintCmdBar(g, null, cmdNextLine, null);
		if (cmdMsg1 != null)
			GameCanvas.paintz.paintCmdBar(g, cmdMsg1, null, cmdMsg2);
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1000)
		{
			try
			{
				GameMidlet.instance.platformRequest((string)p);
			}
			catch (Exception)
			{
			}
			if (!Main.isPC)
				GameMidlet.instance.notifyDestroyed();
			else
				idAction = 1001;
			GameCanvas.endDlg();
		}
		if (idAction == 1001)
		{
			scr = null;
			Char.chatPopup = null;
			serverChatPopUp = null;
			GameScr.info1.isUpdate = true;
			Char.isLockKey = false;
			if (isHavePetNpc)
			{
				GameScr.info1.info.time = 0;
				GameScr.info1.info.info.speed = 10;
			}
		}
		if (idAction != 8000 || performDelay > 0)
			return;
		int num = currChatPopup.currentLine + 1;
		if (num >= currChatPopup.lines.Length)
		{
			Char.chatPopup = null;
			currChatPopup = null;
			GameScr.info1.isUpdate = true;
			Char.isLockKey = false;
			if (nextMultiChatPopUp != null)
			{
				num = 0;
				addChatPopupMultiLine(nextMultiChatPopUp, 100000, nextChar);
				nextMultiChatPopUp = null;
				nextChar = null;
			}
			else
			{
				if (!isHavePetNpc)
					return;
				GameScr.info1.info.time = 0;
				for (int i = 0; i < GameScr.info1.info.infoWaitToShow.size(); i++)
				{
					if (((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed == 10000000)
						((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed = 10;
				}
			}
		}
		else
		{
			ChatPopup chatPopup = addChatPopup(currChatPopup.lines[num], currChatPopup.delay, currChatPopup.c);
			chatPopup.currentLine = num;
			chatPopup.lines = currChatPopup.lines;
			chatPopup.cmdNextLine = currChatPopup.cmdNextLine;
			currChatPopup = chatPopup;
		}
	}
}
