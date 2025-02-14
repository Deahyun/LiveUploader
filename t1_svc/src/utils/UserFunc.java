package utils;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.File;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Set;
import java.util.Vector;

import javax.imageio.ImageIO;

public class UserFunc {
	//
	static String tag = "UserFunc";
	public static char szDeli = 0x18;
	public static String strDeli = String.format("%c", szDeli);
	//
	static int nPushMessageSeq = 0;
	final static int nMaxPushMessageSeq = 10000;
	
	//
	static int nMessageSeq = 0;
	final static int nMaxMessageSeq = 10000;
	// TimeSequence -> TS
	static int nTS = 0;
	final static int nMaxTS = 10000;

	////
	public static boolean V(String strText) {
		if ( strText == null || strText.length() == 0 )
			return false;
		return true;
	}
	
	//
	public static void Sleep(int nMiliSeconds) {
		try {
			Thread.sleep(nMiliSeconds);
		} catch ( Exception e ) {
			e.printStackTrace();
		}
	}
	//
	//
	public static String intTime2String(int nTime) {
		Locale loc = Locale.KOREA;
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", loc);
		Date aDate = new Date(nTime * 1000L);
		String strTime = sdf.format(aDate);
		return strTime;
	}

	//
	public static int now2IntTime() {
		return (int)(System.currentTimeMillis() / 1000L);
	}
	
	//
	public static String getTime() {
		Locale loc = Locale.KOREA;
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", loc);
		Date aDate = new Date(System.currentTimeMillis());
		String strTime = sdf.format(aDate);
		return strTime;
	}
	
	//
	public static String getDate() {
		Locale loc = Locale.KOREA;
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd", loc);
		Date aDate = new Date(System.currentTimeMillis());
		String strTime = sdf.format(aDate);
		return strTime;
	}
	
	//
	public static String getTimeEx() {
		Locale loc = Locale.KOREA;
		SimpleDateFormat sdf = new SimpleDateFormat("yyyyMMdd_HHmmss", loc);
		Date aDate = new Date(System.currentTimeMillis());
		String strTime = sdf.format(aDate);
		return strTime;
	}
	
	////
	public static String toNumberFormat(int num) {
		DecimalFormat df = new DecimalFormat("#,###");
		return df.format(num);
	}
	
	//
	public static long make_message_id() {
		long lResult = 0;
		nPushMessageSeq++;
		if ( nPushMessageSeq >= nMaxPushMessageSeq )
			nPushMessageSeq = 0;
		
		lResult = (now2IntTime() * 10000L) + nPushMessageSeq;
		
		return lResult;
	}
	////
	//
	public static long makeLongId() {
		long lResult = 0;
		nMessageSeq++;
		if ( nMessageSeq >= nMaxMessageSeq )
			nMessageSeq = 0;
		
		lResult = (now2IntTime() * 10000L) + nMessageSeq;
		
		return lResult;
	}
	
	//
	public static String makeTSId() {
		nTS++;
		if ( nTS >= nMaxTS )
			nTS = 0;
		
		return String.format("%s_%04d", getTimeEx(), nTS);
	}
	
	//
	public static String joinString(Vector<String> vString) {
		StringBuilder sb = new StringBuilder();
		//
		for ( int i = 0; i < vString.size(); i++ ) {
			sb.append(vString.get(i));
			if ( i < vString.size() - 1 ) {
				sb.append(szDeli);
			}
		}
		return sb.toString();
	}

	//
	public static Vector<String> splitString(String strSrc) {
		String[] arrResult = strSrc.split(strDeli);
		//
		Vector<String> vString = new Vector<String>();		
		for ( int i = 0; i < arrResult.length; i++ ) {
			vString.add(arrResult[i]);
		}
		return vString;
	}
	
	//
	public static void makeThumbnail(String strPath, String strFile) {
		//
		//UserLog.Log(tag, "path: " + strPath + " file: " + strFile);
		//
		String[] arr = strFile.split("\\.");
		if ( arr.length != 2 ) { return; }
		String strExt = arr[1];
		//
		String strOrg = String.format("%s%s%s", strPath, File.separator, strFile);
		String strThumbnail = String.format("%s%s%s", strPath, File.separator, "tn_" + strFile);
		//UserLog.Log(tag, strOrg + "\n" + strThumbnail);
		//
		final int nW = 360;
		int nH = 1;
		try {
			File org = new File(strOrg);
			File thumb = new File(strThumbnail);
			//
			BufferedImage orgImg = ImageIO.read(org);
			//w1 = orgImg.getWidth();
			//h1 = orgImg.getHeight();
			nH = (int)( (nW * orgImg.getHeight()) / (float)orgImg.getWidth() );
			BufferedImage tnImg = new BufferedImage(nW, nH, BufferedImage.TYPE_3BYTE_BGR);
			Graphics2D gr = tnImg.createGraphics();
			gr.drawImage(orgImg, 0, 0, nW, nH, null);
			ImageIO.write(tnImg, strExt, thumb);
			
		} catch ( Exception e ) {
			e.printStackTrace();
		}
	}
	
	////

	////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	public static List<Integer> stringToIntList(String strArray) {
		List<Integer> lResult = new ArrayList<Integer>();
		if ( strArray == null || strArray.length() < 3 )
			return lResult;

		String strParam = strArray;
		strParam = strParam.replace("{", "");
		strParam = strParam.replace("}", "");
		//strParam = strParam.replace(" ", "");
		String[] arrParam = strParam.split(",");
		//
		//UserLog.Log(tag, "stringToIntList -> [" + strParam + "]");
		if ( arrParam.length == 1 && arrParam[0].equals("") )
			return lResult;
		//
		for ( String strData : arrParam ) {
			int nParam = Integer.parseInt(strData);
			lResult.add(nParam);
		}

		// 순방향 정렬
		//Collections.sort(lResult);
		// 역방향 정렬
		//Collections.reverse(lResult);

		return lResult;
	}

	public static List<String> stringToStringList(String strArray) {
		List<String> lResult = new ArrayList<String>();
		if ( strArray == null || strArray.length() < 3 )
			return lResult;

		String strParam = strArray;
		strParam = strParam.replace("{", "");
		strParam = strParam.replace("}", "");
		String[] arrParam = strParam.split(",");
		//
		//UserLog.Log(tag, "stringToStringList -> [" + strParam + "]");
		if ( arrParam.length == 1 && arrParam[0].equals("") )
			return lResult;
		//
		for ( String strData : arrParam ) {
			lResult.add(strData);
		}

		// 순방향 정렬
		//Collections.sort(lResult);
		// 역방향 정렬
		//Collections.reverse(lResult);

		return lResult;
	}
	
	////
	public static String intListToString(List<Integer> liData) {
		int nSize = liData.size();
		if ( nSize == 0 ) {
			return "{}";
		}

		StringBuilder sb = new StringBuilder();
		sb.append("{");
		for ( int i = 0; i < nSize; i++ ) {
			Integer v = liData.get(i);
			sb.append(v);
			if ( i < nSize - 1 )
				sb.append(",");
		}
		sb.append("}");
		return sb.toString();
	}
	
	//
	public static String stringListToString(List<String> liData) {
		int nSize = liData.size();
		if ( nSize == 0 ) {
			return "{}";
		}

		StringBuilder sb = new StringBuilder();
		sb.append("{");
		for ( int i = 0; i < nSize; i++ ) {
			String v = liData.get(i);
			sb.append(v);
			if ( i < nSize - 1 )
				sb.append(",");
		}
		sb.append("}");
		return sb.toString();
	}

	// array add(+) 연산
	public static List<Integer> addToIntList(List<Integer> toArray, List<Integer> addArray) {
		//
		Set<Integer> sData = new HashSet<Integer>();
		if ( toArray != null )
			sData.addAll(toArray);
		if ( addArray != null )
			sData.addAll(addArray);
		//
		List<Integer> lResult = new ArrayList<Integer>(sData);

		return lResult;
	}

	public static List<String> addToList(List<String> toArray, List<String> addArray) {
		//
		Set<String> sData = new HashSet<String>();
		sData.addAll(toArray);
		sData.addAll(addArray);
		//
		List<String> lResult = new ArrayList<String>(sData);

		return lResult;
	}

	// array minus(-) 연산
	public static List<Integer> removeFromIntList(List<Integer> fromArray, List<Integer> removeArray) {
		if ( fromArray == null ) {
			return null;
		}
		List<Integer> lResult = new ArrayList<Integer>(fromArray);
		if ( removeArray != null )
			lResult.removeAll(removeArray);

		return lResult;
	}

	public static List<String> removeFromList(List<String> fromArray, List<String> removeArray) {
		List<String> lResult = new ArrayList<String>(fromArray);
		lResult.removeAll(removeArray);

		return lResult;
	}

	//////////
}
