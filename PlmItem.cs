using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Aras.IOM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PLM.ArasUtils
{
    public static class Extend
    {
        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static int Count(this Item items)
        {
            if (items == null || items.isError())
            {
                return -1;
            }
            return items.getItemCount();
        }

        /// <summary>
        /// 获取当前对象所有关系类
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item FetchRelationships(this Item item)
        {
            var type = item.getAttribute("type", "");
            return FetchRelationships(item, type);
        }

        /// <summary>
        /// 获取当前对象所有关系类
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item FetchRelationships(this Item item, string type)
        {
            var inn = item.getInnovator();

            if (string.IsNullOrWhiteSpace(type))
            {
                return null;
            }
            var res = inn.applySQL(
                    $"select RELATIONSHIPTYPE.name from innovator.RELATIONSHIPTYPE\r\nwhere RELATIONSHIPTYPE.source_id = (select id from innovator.[itemtype] where name=N'{type}')"
                )
                .ToList();
            foreach (var t in res)
            {
                try
                {
                    var name = t.getProperty("name");
                    var item222 = item.fetchRelationships(name);
                }
                catch { }
            }
            return item;
        }

        /// <summary>
        /// 获取source_id的Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item getItemSourceItem(this Item item)
        {
            var type = item.getPropertyAttribute("source_id", "type", null);

            return getItemSourceItem(item, type);
            ;
        }

        /// <summary>
        /// 获取source_id的Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item getItemSourceItem(this Item item, string type)
        {
            var source_id = item.getProperty("source_id", null);
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(source_id))
            {
                return null;
            }

            var t_item = item.newItem(type, "get");
            t_item.setID(source_id);
            return t_item.apply();
        }

        /// <summary>
        /// 获取related_id的Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item getItemRelatedItem(this Item item)
        {
            var type = item.getPropertyAttribute("related_id", "type", null);
            return getItemRelatedItem(item, type);
        }

        /// <summary>
        /// 获取related_id的Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item getItemRelatedItem(this Item item, string type)
        {
            var related_id = item.getProperty("related_id", null);
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(related_id))
            {
                return null;
            }

            var t_item = item.newItem(type, "get");
            t_item.setID(related_id);
            return t_item.apply();
        }

        /// <summary>
        ///  获取指定属性的Item对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Item getItemPropertyItem(this Item item, string prop)
        {
            var type = item.getPropertyAttribute(prop, "type");
            return getItemPropertyItem(item, prop, type);
        }

        /// <summary>
        /// 获取指定属性的Item对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="prop"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item getItemPropertyItem(this Item item, string prop, string type)
        {
            var id = item.getProperty(prop, "");
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var t_item = item.getItemById(type, id);
            return t_item;
        }

        /// <summary>
        /// 根据类型和ID获取对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Item getItemById(this Item item, string type, string id)
        {
            var t_item = item.newItem(type, "get");
            t_item.setID(id);
            t_item = t_item.apply();
            if (t_item.isError())
            {
                return null;
            }
            return t_item;
        }

        /// <summary>
        /// 获取keyed_name
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string getKeyedName(this Item item)
        {
            return item.getProperty("keyed_name", "");
        }

        /// <summary>
        /// Items转List
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<Item> ToList(this Item items)
        {
            if (items.isError())
            {
                return new List<Item>();
            }
            if (items.node == null && items.nodeList == null)
            {
                return new List<Item>();
            }
            if (items.getItemCount() <= 0)
            {
                return new List<Item>();
            }

            return items.Enumerator().ToList();
        }

        /// <summary>
        /// 检查错误，并且抛出异常
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Item CheckError(this Item item)
        {
            if (item.isError())
            {
                throw new Exception(item.getErrorString());
            }
            return item;
        }

        /// <summary>
        /// Items转List
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<Item>? Enumerator(this Item items)
        {
            if (items.isError())
            {
                yield return default;
            }
            if (items.nodeList != null)
            {
                for (int i = 0; i < items.getItemCount(); i++)
                {
                    var item = items.getItemByIndex(i);
                    var newItem = item.newItem();
                    //newItem.node = item.node;
                    //newItem.dom=new System.Xml.XmlDocument();
                    //newItem.dom.AppendChild(newItem.node);
                    newItem.loadAML(item.node.OuterXml);
                    yield return newItem;
                }
            }
            else
            {
                yield return items;
            }
        }

        /// <summary>
        /// 移除指定属性
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>移除属性后的Item集合</returns>
        public static IEnumerable<Item> removeAttribute(
            this IEnumerable<Item> items,
            string attributeName
        )
        {
            return items.Select(l =>
            {
                l.removeAttribute(attributeName);
                return l;
            });
            ;
        }

        /// <summary>
        /// Items转List
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<Item> Clone(this IEnumerable<Item> items, bool cloneRelationships)
        {
            return items.Select(l => l.clone(cloneRelationships));
        }

        /// <summary>
        /// 获取值，同getProperty
        /// </summary>
        /// <param name="item"></param>
        /// <param name="propName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string getValue(this Item item, string propName, string defaultValue = "")
        {
            return item.getProperty(propName, defaultValue);
        }

        /// <summary>
        /// 设置值，同setProperty
        /// </summary>
        /// <param name="item"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item setValue(this Item item, string propName, string value)
        {
            item.setProperty(propName, value);
            return item;
        }

        /// <summary>
        /// 获取值，同item.getProperty<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propName"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static T getValue<T>(this Item item, string propName, T defaultvalue = default) where T : struct
        {
            var value = item.getProperty(propName, null);
            if (value == null)
            {
                return defaultvalue;
            }
            return ConvertTo<T>(value);
        }

        /// <summary>
        /// 转换类型函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static T ConvertTo<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            Type type = typeof(T);

            if (type == typeof(int))
            {
                return (T)(object)int.Parse(value);
            }
            else if (type == typeof(double))
            {
                return (T)(object)double.Parse(value);
            }
            else if (type == typeof(decimal))
            {
                return (T)(object)decimal.Parse(value);
            }
            else if (type == typeof(DateTime))
            {
                return (T)(object)DateTime.Parse(value);
            }
            else
            {
                throw new NotSupportedException("Unsupported type conversion");
            }
        }

        /// <summary>
        /// itemtype:List<Item[@type='Property']>
        /// </summary>
        static Dictionary<string, List<Item>> dictPropertys = new Dictionary<string, List<Item>>();

        /// <summary>
        /// 获取list的Label，若无Lable,返回Value
        /// </summary>
        /// <param name="itemtypeName"></param>
        /// <param name="propertyName"></param>
        /// <param name="Value"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetListLabelByLang(
            this Item thisItem,
            string propertyName,
            string lang = "zc"
        )
        {
            var Value = thisItem.getProperty(propertyName, "");

            var inn = thisItem.getInnovator();
            if (string.IsNullOrWhiteSpace(Value))
            {
                return Value;
            }
            var propertyItme = thisItem.GetItemTypePropertys(propertyName).FirstOrDefault();
            if (propertyItme.isError() || propertyItme.getItemCount() == 0)
            {
                throw new Exception("属性名称:" + propertyName + "未找到!");
            }
            string data_type = propertyItme.getProperty("data_type");
            string data_source = propertyItme.getProperty("data_source");
            if (data_type.ToLower() == "list")
            {
                Item listItem = inn.getItemById("list", data_source);
                Item ValueItems = inn.newItem("value", "get");
                ValueItems.setProperty("source_id", listItem.getID());
                ValueItems = ValueItems.apply();
                for (int i = 0; i < ValueItems.getItemCount(); i++)
                {
                    Item ValueItem = ValueItems.getItemByIndex(i);
                    string value = ValueItem.getProperty("value", "");
                    if (value == Value)
                    {
                        var res = "";
                        if (string.IsNullOrWhiteSpace(lang))
                        {
                            res = ValueItem.getProperty("label", "");
                        }
                        else
                        {
                            res = ValueItem.getProperty("label", "", lang);
                        }
                        if (res == "")
                        {
                            return Value;
                        }
                        else
                        {
                            return res;
                        }
                    }
                }
                //return Value;
            }
            return "";
        }

        /// <summary>
        /// 获取对象类的所有Property
        /// </summary>
        /// <param name="thisItem"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<Item> GetItemTypePropertys(this Item thisItem, string propertyName = "")
        {
            var inn = thisItem.getInnovator();
            var itemtypeName = thisItem.getType();
            var itemtypeID = thisItem.getAttribute("typeId");

            if (!dictPropertys.ContainsKey(itemtypeName))
            {
                var itemtypeItem = inn.newItem("itemtype", "get");
                itemtypeItem.setProperty("name", itemtypeName);
                itemtypeItem = itemtypeItem.apply();
                var propertyItme = inn.newItem("property", "get");
                propertyItme.setProperty("source_id", itemtypeItem.getID());
                propertyItme = propertyItme.apply();

                dictPropertys.Add(itemtypeName, propertyItme.ToList());
            }
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return dictPropertys[itemtypeName];
            }
            else
            {
                return dictPropertys[itemtypeName]
                    .Where(l => l.getProperty("name", "") == propertyName)
                    .ToList();
            }
        }

        /// <summary>
        ///获取FilterList的label
        /// </summary>
        /// <param name="listid">listID,List的ID</param>
        /// <param name="key">fileterlist的Value</param>
        /// <param name="prop">list的Value</param>
        /// <returns></returns>
        public static string GetFilterListLabel(
            this Item thisItem,
            string propertyName,
            string lang = "zc"
        )
        {
            var Value = thisItem.getProperty(propertyName, "");
            var key = Value;
            var itemtypeName = thisItem.getType();
            var inn = thisItem.getInnovator();
            if (string.IsNullOrWhiteSpace(Value))
            {
                return Value;
            }
            var propertyItme = thisItem.GetItemTypePropertys(propertyName).FirstOrDefault();
            if (propertyItme.isError() || propertyItme.getItemCount() == 0)
            {
                throw new Exception("属性名称:" + propertyName + "未找到!");
            }
            string data_type = propertyItme.getProperty("data_type");
            string data_source = propertyItme.getProperty("data_source");
            string pattern = propertyItme.getProperty("pattern");
            var prop = pattern;
            var listid = data_source;
            if (string.IsNullOrWhiteSpace(listid) || string.IsNullOrWhiteSpace(key))
            {
                return "";
            }
            var filter = thisItem.getProperty(prop, "");
            // var item = inn.getItemById("list", listid);
            var values = inn.newItem("Filter Value", "get");
            values.setProperty("source_id", listid);
            values.setProperty("value", key);
            values.setProperty("filter", filter);
            values = values.apply();
            if (!values.isError() && values.getItemCount() > 0)
            {
                for (int i = 0; i < values.getItemCount(); i++)
                {
                    var valueItem = values.getItemByIndex(i);
                    if (valueItem.getProperty("value") == key)
                    {
                        var str = valueItem.getProperty("label", "", lang);
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            return key;
                        }
                        else
                        {
                            return str;
                        }
                    }
                }
            }
            return key;
        }

        /// <summary>
        /// 获取KeyedName
        /// </summary>
        /// <param name="thisItem"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetItemKeyName(
            this Item thisItem,
            string propertyName,
            string defaultValue = "",
            string lang = null
        )
        {
            //string propertyName, string defaultValue, string lang
            return thisItem.getPropertyAttribute(propertyName, "keyed_name", defaultValue, lang);
        }

        /// <summary>
        /// 转为一个Item
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static Item ToItems(this IEnumerable<Item> listItem)
        {
            var t_item = listItem.FirstOrDefault()?.newItem() ?? null;
            var result =
                $"<Result>{string.Join("", listItem.Select(l => l.node.OuterXml))}</Result>";
            t_item?.loadAML(result);
            return t_item;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> apply(
            this IEnumerable<Item> listItem,
            string action = default
        )
        {
            if (action == default)
            {
                //Innovator inn = listItem.First().getInnovator();
                //var aml = listItem.Select(l => l.node.OuterXml);
                //return inn.applyAML($"<AML>{string.Join("", aml)}</AML>");
                return listItem.Select(l => l.apply());
            }
            else
            {
                return listItem.Select(l => l.apply(action));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setAction(this IEnumerable<Item> listItem, string action)
        {
            return listItem.Select(l =>
            {
                l.setAction(action);
                return l;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setAttribute(
            this IEnumerable<Item> listItem,
            string attributeName,
            string attributeValue
        )
        {
            return listItem.Select(l =>
            {
                l.setAttribute(attributeName, attributeValue);
                return l;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setNewID(this IEnumerable<Item> listItem)
        {
            return listItem.Select(l =>
            {
                l.setNewID();
                return l;
            });
        }

        /// <summary>
        /// 设置属性Item,需要注意，返回值为this，IOM默认提供返回值为传递进来的item
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setPropertyItem(
            this IEnumerable<Item> listItem,
            string propertyName,
            Item item
        )
        {
            return listItem.Select(l =>
            {
                l.setPropertyItem(propertyName, item);
                return l;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setType(
            this IEnumerable<Item> listItem,
            string itemTypeName
        )
        {
            return listItem.Select(l =>
            {
                l.setType(itemTypeName);
                return l;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setId(this IEnumerable<Item> listItem, List<string> ids)
        {
            if (listItem.Count() != ids.Count)
            {
                throw new Exception("设置ID时，传递的参数错误，数量不一致");
            }

            return listItem.Select(
                (value, index) =>
                {
                    value.setID(ids[index]);
                    return value;
                }
            );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> CheckError(this IEnumerable<Item> listItem)
        {
            return listItem.Select(l =>
            {
                l.CheckError();
                return l;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<string> getErrorString(this IEnumerable<Item> listItem)
        {
            return listItem.Select(l =>
            {
                return l.getErrorString();
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<string> getID(this IEnumerable<Item> listItem)
        {
            return listItem.Select(l =>
            {
                return l.getID();
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<string> getResult(this IEnumerable<Item> listItem)
        {
            return listItem.Select(l =>
            {
                return l.getResult();
            });
        }

        /// <summary>
        /// 设置ID,适用于使用SQL查询后，需要对Attribute设置ID时的场景
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> setId(this IEnumerable<Item> listItem)
        {
            return listItem.Select(
                (item) =>
                {
                    item.setID(item.getProperty("id"));
                    return item;
                }
            );
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public static IEnumerable<Item> SetProperty(
            this IEnumerable<Item> listItem,
            string prop,
            string value,
            string lang = default
        )
        {
            return listItem.Select(
                (item) =>
                {
                    if (string.IsNullOrWhiteSpace(lang))
                    {
                        item.setProperty(prop, value);
                    }
                    else
                    {
                        item.setProperty(prop, value, lang);
                    }
                    return item;
                }
            );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getPropertys(this Item item)
        {
            var doc = XDocument.Parse(item.node.OuterXml);
            return doc.Descendants()
                .ToDictionary(
                    l => l.Name.LocalName,
                    l =>
                    {
                        return l.Attribute("keyed_name")?.Value ?? l.Value;
                        // l.Value;
                    }
                );
        }

        public static JObject ToJObject(this string aa)
        {
            return JObject.Parse(aa);
        }

        public static string ToJson(this object aa)
        {
            if (aa is null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(aa);
        }

        public static JArray ToJArrary(this string aa)
        {
            return JArray.Parse(aa);
        }

        /// <summary>
        /// 获取属性并且转为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static T getProperty<T>(
            this Item item,
            string propertyName,
            string defaultValue,
            string lang
        )
            where T : struct
        {
            var value = item.getProperty(propertyName, defaultValue, lang);
            // 尝试将value转换为T类型
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 获取属性并且转为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T getProperty<T>(this Item item, string propertyName, string defaultValue)
            where T : struct
        {
            var value = item.getProperty(propertyName, defaultValue);
            // 尝试将value转换为T类型
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 获取属性并且转为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T getProperty<T>(this Item item, string propertyName)
            where T : struct
        {
            var value = item.getProperty(propertyName);
            // 尝试将value转换为T类型
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    /// <summary>
    /// IEnumerable<Item> 扩展方法
    /// </summary>
    public static class IEnumerableItemExtensions
    {
        /// <summary>
        /// 根据属性值过滤Item集合
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>过滤后的Item集合</returns>
        public static IEnumerable<Item> WhereProperty(this IEnumerable<Item> items, string propertyName, string value)
        {
            return items.Where(item => item.getProperty(propertyName, "") == value);
        }

        /// <summary>
        /// 根据属性值过滤Item集合（支持泛型）
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>过滤后的Item集合</returns>
        public static IEnumerable<Item> WhereProperty<T>(this IEnumerable<Item> items, string propertyName, T value) where T : struct
        {
            return items.Where(item =>
            {
                try
                {
                    var itemValue = item.getProperty<T>(propertyName);
                    return EqualityComparer<T>.Default.Equals(itemValue, value);
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 根据属性包含内容过滤Item集合
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="containsValue">包含的值</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>过滤后的Item集合</returns>
        public static IEnumerable<Item> WherePropertyContains(this IEnumerable<Item> items, string propertyName, string containsValue, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return items.Where(item => item.getProperty(propertyName, "").IndexOf(containsValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return items.Where(item => item.getProperty(propertyName, "").Contains(containsValue));
        }

        /// <summary>
        /// 根据属性值范围过滤Item集合
        /// </summary>
        /// <typeparam name="T">属性值类型（可比较）</typeparam>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>过滤后的Item集合</returns>
        public static IEnumerable<Item> WherePropertyInRange<T>(this IEnumerable<Item> items, string propertyName, T minValue, T maxValue) where T : struct, IComparable<T>
        {
            return items.Where(item =>
            {
                try
                {
                    var itemValue = item.getProperty<T>(propertyName);
                    return itemValue.CompareTo(minValue) >= 0 && itemValue.CompareTo(maxValue) <= 0;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 获取第一个符合条件的Item或默认值
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>第一个符合条件的Item或默认值</returns>
        public static Item FirstOrDefaultByProperty(this IEnumerable<Item> items, string propertyName, string value)
        {
            return items.WhereProperty(propertyName, value).FirstOrDefault();
        }

        /// <summary>
        /// 根据属性去重
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>去重后的Item集合</returns>
        public static IEnumerable<Item> DistinctByProperty(this IEnumerable<Item> items, string propertyName)
        {
            var seen = new HashSet<string>();
            return items.Where(item => seen.Add(item.getProperty(propertyName, "")));
        }

        /// <summary>
        /// 转换为以ID为键的字典
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <returns>以ID为键的字典</returns>
        public static Dictionary<string, Item> ToDictionary(this IEnumerable<Item> items)
        {
            return items.Where(item => !string.IsNullOrEmpty(item.getID()))
                       .ToDictionary(item => item.getID(), item => item);
        }

        /// <summary>
        /// 转换为以指定属性为键的字典
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>以指定属性为键的字典</returns>
        public static Dictionary<string, Item> ToDictionaryByProperty(this IEnumerable<Item> items, string propertyName)
        {
            return items.ToDictionary(item => item.getProperty(propertyName, ""), item => item);
        }

        /// <summary>
        /// 选择特定属性值
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<string> SelectProperty(this IEnumerable<Item> items, string propertyName)
        {
            return items.Select(item => item.getProperty(propertyName, ""));
        }

        /// <summary>
        /// 选择特定属性值（泛型版本）
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<T> SelectProperty<T>(this IEnumerable<Item> items, string propertyName) where T : struct
        {
            return items.Select(item =>
            {
                try
                {
                    return item.getProperty<T>(propertyName);
                }
                catch
                {
                    return default(T);
                }
            });
        }

        /// <summary>
        /// 选择多个属性值
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyNames">属性名列表</param>
        /// <returns>属性值字典集合</returns>
        public static IEnumerable<Dictionary<string, string>> SelectProperties(this IEnumerable<Item> items, params string[] propertyNames)
        {
            return items.Select(item =>
            {
                var dict = new Dictionary<string, string>();
                foreach (var propName in propertyNames)
                {
                    dict[propName] = item.getProperty(propName, "");
                }
                return dict;
            });
        }

        /// <summary>
        /// 批量应用特定动作
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="action">动作类型</param>
        /// <returns>应用动作后的Item集合</returns>
        public static IEnumerable<Item> ApplyAction(this IEnumerable<Item> items, string action)
        {
            return items.Select(item =>
            {
                item.setAction(action);
                return item;
            });
        }

        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>更新后的Item集合</returns>
        public static IEnumerable<Item> BatchUpdate(this IEnumerable<Item> items, string propertyName, string value)
        {
            return items.SetProperty(propertyName, value);
        }

        /// <summary>
        /// 批量更新多个属性
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="properties">属性字典</param>
        /// <returns>更新后的Item集合</returns>
        public static IEnumerable<Item> BatchUpdate(this IEnumerable<Item> items, Dictionary<string, string> properties)
        {
            return items.Select(item =>
            {
                foreach (var prop in properties)
                {
                    item.setProperty(prop.Key, prop.Value);
                }
                return item;
            });
        }

        /// <summary>
        /// 验证Item集合中的所有Item
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="validator">验证函数</param>
        /// <returns>验证通过的Item集合</returns>
        public static IEnumerable<Item> Validate(this IEnumerable<Item> items, Func<Item, bool> validator)
        {
            return items.Where(validator);
        }

        /// <summary>
        /// 按属性分组计数
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>分组计数结果</returns>
        public static Dictionary<string, int> CountByProperty(this IEnumerable<Item> items, string propertyName)
        {
            return items.GroupBy(item => item.getProperty(propertyName, ""))
                       .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// 按属性分组
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>分组结果</returns>
        public static IEnumerable<IGrouping<string, Item>> GroupByProperty(this IEnumerable<Item> items, string propertyName)
        {
            return items.GroupBy(item => item.getProperty(propertyName, ""));
        }

        /// <summary>
        /// 检查集合中是否有错误项
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <returns>是否有错误</returns>
        public static bool HasErrors(this IEnumerable<Item> items)
        {
            return items.Any(item => item.isError());
        }

        /// <summary>
        /// 获取有效项（非错误项）
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <returns>有效项集合</returns>
        public static IEnumerable<Item> GetValidItems(this IEnumerable<Item> items)
        {
            return items.Where(item => !item.isError());
        }

        /// <summary>
        /// 获取错误项
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <returns>错误项集合</returns>
        public static IEnumerable<Item> GetErrorItems(this IEnumerable<Item> items)
        {
            return items.Where(item => item.isError());
        }

        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this IEnumerable<Item> items)
        {
            var dataList = items.Select(item => item.getPropertys()).ToList();
            return JsonConvert.SerializeObject(dataList, Formatting.Indented);
        }

        /// <summary>
        /// 转换为JSON字符串（指定属性）
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyNames">要包含的属性名列表</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this IEnumerable<Item> items, params string[] propertyNames)
        {
            var dataList = items.Select(item =>
            {
                var dict = new Dictionary<string, string>();
                foreach (var propName in propertyNames)
                {
                    dict[propName] = item.getProperty(propName, "");
                }
                return dict;
            }).ToList();
            return JsonConvert.SerializeObject(dataList, Formatting.Indented);
        }

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页后的Item集合</returns>
        public static IEnumerable<Item> Page(this IEnumerable<Item> items, int pageIndex, int pageSize)
        {
            return items.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页信息</returns>
        public static (int totalCount, int totalPages) GetPageInfo(this IEnumerable<Item> items, int pageSize)
        {
            var totalCount = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return (totalCount, totalPages);
        }

        /// <summary>
        /// 获取集合中所有Item的指定属性值
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<string> getProperty(this IEnumerable<Item> items, string propertyName, string defaultValue = "")
        {
            return items.Select(item => item.getProperty(propertyName, defaultValue));
        }

        /// <summary>
        /// 获取集合中所有Item的指定属性值（泛型版本）
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<T> getProperty<T>(this IEnumerable<Item> items, string propertyName) where T : struct
        {
            return items.Select(item =>
            {
                try
                {
                    return item.getProperty<T>(propertyName);
                }
                catch
                {
                    return default(T);
                }
            });
        }

        /// <summary>
        /// 获取集合中所有Item的指定属性值（带默认值的泛型版本）
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<T> getProperty<T>(this IEnumerable<Item> items, string propertyName, T defaultValue) where T : struct
        {
            return items.Select(item =>
            {
                try
                {
                    return item.getProperty<T>(propertyName);
                }
                catch
                {
                    return defaultValue;
                }
            });
        }

        /// <summary>
        /// 获取集合中所有Item的指定属性值（带语言版本）
        /// </summary>
        /// <param name="items">Item集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="lang">语言代码</param>
        /// <returns>属性值集合</returns>
        public static IEnumerable<string> getProperty(this IEnumerable<Item> items, string propertyName, string defaultValue, string lang)
        {
            return items.Select(item => item.getProperty(propertyName, defaultValue, lang));
        }
    }

    public static class ItemPropertyExtend
    {
        public static T CreatedById<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("created_by_id", defaultValue);
        }

        public static T CreatedOn<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("created_on", defaultValue);
        }

        public static T Classification<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("classification", defaultValue);
        }

        public static T KeyedName<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("keyed_name", defaultValue);
        }

        public static T Id<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("id", defaultValue);
        }

        public static T OwnedById<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("owned_by_id", defaultValue);
        }

        public static T ManagedById<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("managed_by_id", defaultValue);
        }

        public static T ModifiedOn<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("modified_on", defaultValue);
        }

        public static T ModifiedById<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("modified_by_id", defaultValue);
        }

        public static T CurrentState<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("current_state", defaultValue);
        }

        public static T State<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("state", defaultValue);
        }

        public static T LockedById<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("locked_by_id", defaultValue);
        }

        public static T IsCurrent<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("is_current", defaultValue);
        }

        public static T MajorRev<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("major_rev", defaultValue);
        }

        public static T MinorRev<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("minor_rev", defaultValue);
        }

        public static T IsReleased<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("is_released", defaultValue);
        }

        public static T NotLockable<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("not_lockable", defaultValue);
        }

        public static T Css<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("css", defaultValue);
        }

        public static T Generation<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("generation", defaultValue);
        }

        public static T NewVersion<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("new_version", defaultValue);
        }

        public static T ConfigId<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("config_id", defaultValue);
        }

        public static T PermissionId<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("permission_id", defaultValue);
        }

        public static T TeamId<T>(this Item item, T defaultValue = default) where T : struct
        {
            return item.getValue<T>("team_id", defaultValue);
        }
    }
}
