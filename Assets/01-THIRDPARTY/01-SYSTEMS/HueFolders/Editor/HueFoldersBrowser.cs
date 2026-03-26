using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

//  HueFolders © NullTale - https://twitter.com/NullTale/
namespace HueFolders
{
    public static class HueFoldersBrowser
    {
        private static GUIStyle s_labelNormal;
        private static GUIStyle s_labelSelected;
        private static string s_pendingSelection;
        private static bool s_SelectionIn;
        
        // =======================================================================
        public static void FolderColorization(string guid, Rect rect)
        {
            var inTreeViewOnly = true; //SettingsProvider.s_InTreeViewOnly.Get<bool>();
            if (inTreeViewOnly && _isTreeView() == false)
                return;
            
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (_isValidFolder(path)) 
                return;
            
            var labelOverride = true; //SettingsProvider.s_LabelOverride.Get<bool>();

            // get base color of folder, configure rect
            var data = _getFolderData(out var isSubFolder);
            var baseColor = data == null ? SettingsProvider.s_FoldersDefaultTint : data._color;
            if (baseColor.a <= 0f)
                return;
            
            var folderColor = baseColor * SettingsProvider.s_FoldersTint.Get<Color>();
            if (isSubFolder)
            {
                var tint = SettingsProvider.s_SubFoldersTint.Get<Color>(); 
                folderColor *= tint;
            }
            
            var isSmall = rect.width > rect.height;
            if (isSmall == false)
                return;

            if (_isTreeView() == false)
                rect.xMin += 3;
            var bgColor = EditorGUIUtility.isProSkin 
                ? new Color(0.219f, 0.219f, 0.219f, 1f) // pro skin background
                : new Color(0.76f, 0.76f, 0.76f, 1f);   // light skin background

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                s_pendingSelection = guid;
                GUI.changed = true;
            }
            
            if (labelOverride)
            {
                EditorGUI.DrawRect(_iconRect(), bgColor);
                EditorGUI.DrawRect(_textRectNoFix(), bgColor);
            }

            var isGuidSelected = Selection.assetGUIDs.Contains(guid);
            var isSelected = (s_pendingSelection == null && isGuidSelected) || s_pendingSelection == guid;
            
            if (isGuidSelected && s_pendingSelection == guid)
                s_pendingSelection = null;

            if (isSelected && EditorGUIUtility.editingTextField == false)
            {
                var selColor = EditorGUIUtility.isProSkin
                    ? new Color(62 / 255f, 95 / 255f, 150 / 255f)
                    : new Color(58 / 255f, 121 / 255f, 187 / 255f);
                
                var col = SettingsProvider.s_SelectionTint.Get<Color>();
                selColor = Color.Lerp(selColor, col, col.a);
                
                var fullRect = new Rect(rect);
                fullRect.xMin = 0f;
                fullRect.xMax = Screen.width;

                GUI.color = selColor;
                GUI.DrawTexture(fullRect, Texture2D.whiteTexture);
            }
            
            // draw background, overdraw icon and text
            GUI.color = folderColor;
            GUI.DrawTexture(rect, _gradient(), ScaleMode.ScaleAndCrop);
            
            GUI.color = Color.white;
            GUI.DrawTexture(_iconRect(), _folderIcon());
            
            if (labelOverride)
            {
                var textRect = _textRect();
                GUI.Label(textRect, Path.GetFileName(path), _labelSkin());
                
                if (_isTreeView())
                {
                    textRect.x += .5f;
                    textRect.y -= .5f;
                    GUI.Label(textRect, Path.GetFileName(path), _labelSkin());
                }
            }

            GUI.color = Color.white;
            
            // =======================================================================
            bool _isValidFolder(string path)
            {
                return AssetDatabase.IsValidFolder(path) == false || path.StartsWith("Packages") || path.Equals("Assets");
            }
            
            /*bool _isFolderOpened()
            {
                if (_isTreeView() == false)
                    return false;

                if (_isSelected())
                    return true;

                return SettingsProvider.s_FoldersDataDic.Keys.Any(g =>
                {
                    var otherPath = AssetDatabase.GUIDToAssetPath(g);
                    return otherPath.StartsWith(path + "/");
                });
            }*/
            
            SettingsProvider.FolderData _getFolderData(out bool isSubFolder)
            {
                isSubFolder = false;        
                if (SettingsProvider.s_FoldersDataDic.TryGetValue(guid, out var folderData))
                    return folderData;
                
                isSubFolder = true;        
                var searchPath = path;
                while (folderData == null)
                {
                    searchPath = Path.GetDirectoryName(searchPath);
                    if (string.IsNullOrEmpty(searchPath))
                        return null;
                    
                    var searchGuid = AssetDatabase.GUIDFromAssetPath(searchPath).ToString();
                    
                    SettingsProvider.s_FoldersDataDic.TryGetValue(searchGuid, out folderData);
                    if (folderData != null && folderData._recursive == false)
                        return null;
                }
                
                return folderData;
            }
            
            Rect _iconRect()
            {
                var result = new Rect(rect);
                result.width = result.height;

                return result;
            }
            
            Rect _textRect()
            {
                var result = new Rect(rect);
                result.xMin += _iconRect().width;
                if (_isTreeView())
                {
                    result.yMax -= 1;
                }

                return result;
            }
            
            Rect _textRectNoFix()
            {
                var result = new Rect(rect);
                result.xMin += _iconRect().width;

                return result;
            }
            
			Texture _folderIcon()
			{
                if (EditorGUIUtility.isProSkin)
					return EditorGUIUtility.IconContent(_isFolderEmpty() ? "FolderEmpty Icon" : "Folder Icon").image;
				else
                {
                    if (_isSelected())
                        return EditorGUIUtility.IconContent(_isFolderEmpty() ? "FolderEmpty On Icon" : "Folder On Icon").image;
                    else
					    return EditorGUIUtility.IconContent(_isFolderEmpty() ? "FolderEmpty Icon" : "Folder Icon").image;
                }
			}
			
            bool _isFolderEmpty()
            {
                var items = Directory.EnumerateFileSystemEntries(path);
                using (var en = items.GetEnumerator())
                    return en.MoveNext() == false;
            }
            
            bool _isSelected()
            {
                return Selection.assetGUIDs.Contains(guid);
            }
            
            bool _isTreeView()
            {
                return (rect.x - 16) % 14 == 0;
            }
            
            GUIStyle _labelSkin()
            {
                if (s_labelSelected == null)
                {
                    s_labelSelected                  = new GUIStyle("Label");
                    s_labelSelected.normal.textColor = Color.white;
                    s_labelSelected.hover.textColor  = s_labelSelected.normal.textColor;
                }
                if (s_labelNormal == null)
                {
                    s_labelNormal                  = new GUIStyle("Label");
                    s_labelNormal.normal.textColor = EditorGUIUtility.isProSkin ? new Color32(175, 175, 175, 255) : new Color32(2, 2, 2, 255);
                    s_labelNormal.hover.textColor  = s_labelNormal.normal.textColor;
                }

                return _isSelected() ? s_labelSelected : s_labelNormal;
            }

            Texture2D _gradient()
            {
                if (SettingsProvider.s_Gradient == null)
                    SettingsProvider._updateGradient();
                
                //if (_isSelected())
                //    return SettingsProvider.s_Fill;
                
                return SettingsProvider.s_Gradient;
            }
        }
    }
}