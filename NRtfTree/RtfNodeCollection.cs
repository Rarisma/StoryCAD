/********************************************************************************
 *   This file is part of NRtfTree Library.
 *
 *   NRtfTree Library is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   NRtfTree Library is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 ********************************************************************************/

/********************************************************************************
 * Library:		NRtfTree
 * Version:     v0.3.0
 * Date:		02/09/2007
 * Copyright:   2007 Salvador Gomez
 * E-mail:      sgoliver.net@gmail.com
 * Home Page:	http://www.sgoliver.net
 * SF Project:	http://nrtftree.sourceforge.net
 *				http://sourceforge.net/projects/nrtftree
 * Class:		RtfNodeCollection
 * Description:	Colecci�n de nodos de un �rbol RTF.
 * ******************************************************************************/

using System.Collections;

namespace NRtfTree
{
    namespace Core
    {
        /// <summary>
        /// Colecci�n de nodos de un documento RTF.
        /// </summary>
        public class RtfNodeCollection : CollectionBase
        {
            #region M�todos Publicos

            /// <summary>
            /// A�ade un nuevo nodo a la colecci�n actual.
            /// </summary>
            /// <param name="node">Nuevo nodo a a�adir.</param>
            /// <returns>Posici�n en la que se ha insertado el nuevo nodo.</returns>
            public int Add(RtfTreeNode node)
            {
                InnerList.Add(node);

                return InnerList.Count - 1;
            }

            /// <summary>
            /// Inserta un nuveo nodo en una posici�n determinada de la colecci�n.
            /// </summary>
            /// <param name="index">Posici�n en la que insertar el nodo.</param>
            /// <param name="node">Nuevo nodo a insertar.</param>
            public void Insert(int index, RtfTreeNode node)
            {
                InnerList.Insert(index, node);
            }

            /// <summary>
            /// Indizador de la clase RtfNodeCollection. 
            /// Devuelve el nodo que ocupa la posici�n 'index' dentro de la colecci�n.
            /// </summary>
            public RtfTreeNode this[int index]
            {
                get => (RtfTreeNode)InnerList[index];
                set => InnerList[index] = value;
            }

            /// <summary>
            /// Devuelve el �ndice del nodo pasado como par�metro dentro de la lista de nodos de la colecci�n.
            /// </summary>
            /// <param name="node">Nodo a buscar en la colecci�n.</param>
            /// <returns>Indice del nodo buscado. Devolver� el valor -1 en caso de no encontrarse el nodo dentro de la colecci�n.</returns>
            public int IndexOf(RtfTreeNode node)
            {
                return InnerList.IndexOf(node);
            }

            /// <summary>
            /// A�ade al final de la colecci�n una nueva lista de nodos.
            /// </summary>
            /// <param name="collection">Nueva lista de nodos a a�adir a la colecci�n actual.</param>
            public void AddRange(RtfNodeCollection collection)
            {
                InnerList.AddRange(collection);
            }

            /// <summary>
            /// Elimina un conjunto de nodos adyacentes de la colecci�n.
            /// </summary>
            /// <param name="index">�ndice del primer nodo del conjunto a eliminar.</param>
            /// <param name="count">N�mero de nodos a eliminar.</param>
            public void RemoveRange(int index, int count)
            {
                InnerList.RemoveRange(index, count);
            }

            #endregion
        }
    }
}
